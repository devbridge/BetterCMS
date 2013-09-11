using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.SortPageContent
{
    public class SortPageContentCommand : CommandBase, ICommand<PageContentSortViewModel, SortPageContentResponse>
    {
        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortPageContentCommand" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public SortPageContentCommand(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SortPageContentResponse Execute(PageContentSortViewModel request)
        {
            var response = new SortPageContentResponse();
            
            UnitOfWork.BeginTransaction();

            #region Updated page content Order if needed.

            var pageQuery = Repository.AsQueryable<PageContent>().Where(f => f.Page.Id == request.PageId && f.Region.Id == request.RegionId);
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                pageQuery = pageQuery.Fetch(f => f.Page).ThenFetchMany(f => f.AccessRules);
            }
            var pageContents = pageQuery.ToList();

            // Demand access
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                if (pageContents.Count > 0)
                {
                    var page = pageContents[0].Page;

                    AccessControlService.DemandAccess(page, Context.Principal, AccessLevel.ReadWrite, RootModuleConstants.UserRoles.EditContent);
                }
            }
            else
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            }
            
            var index = 0;
            foreach (var content in request.PageContents)
            {
                var pageContent = pageContents.FirstOrDefault(f => f.Id == content.Id);
                if (pageContent == null)
                {
                    throw new EntityNotFoundException(typeof(PageContent), Guid.Empty);
                }
                if (pageContent.Order != index)
                {
                    if (pageContent.Version != content.Version)
                    {
                        throw new ConcurrentDataException(pageContent);
                    }

                    pageContent.Order = index;
                    Repository.Save(pageContent);
                    response.UpdatedPageContents.Add(new ContentViewModel { Id = pageContent.Id });
                }
                index++;
            }
            #endregion

            #region Get page content updated versions.
            // NOTE: Maybe, it is logical to place this code after UnitOfWork.Commit(), but in this way, NHibernate generates less traffic to DB.
            pageContents = Repository.AsQueryable<PageContent>().Where(f => f.Page.Id == request.PageId && f.Region.Id == request.RegionId).ToList();
            foreach (var updatedPageContent in response.UpdatedPageContents)
            {
                var pageContent = pageContents.FirstOrDefault(f => f.Id == updatedPageContent.Id);
                if (pageContent == null)
                {
                    throw new CmsException(string.Format("Content was not found by id={0}.", updatedPageContent.Id));
                }
                updatedPageContent.Version = pageContent.Version;
            }
            #endregion

            UnitOfWork.Commit();

            return response;
        }
    }
}