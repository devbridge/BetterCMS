using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.SortPageContent
{
    public class SortPageContentCommand : CommandBase, ICommand<PageContentSortViewModel, SortPageContentCommandResponse>
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
        public SortPageContentCommandResponse Execute(PageContentSortViewModel request)
        {
            UnitOfWork.BeginTransaction();

            var sortedPageContents = new List<PageContent>();
            var regionIds = request.PageContents.Select(r => r.RegionId).Distinct().ToArray();
            var parentPageContentIds = request.PageContents.Where(r => r.ParentPageContentId.HasValue).Select(r => r.ParentPageContentId.Value);
            var pageContentIds = request.PageContents.Select(r => r.PageContentId).Concat(parentPageContentIds).Distinct().ToArray();
            var response = new SortPageContentCommandResponse { PageContents = request.PageContents };

            // Load all page contents from all regions from request
            var contentsFuture = Repository
                .AsQueryable<PageContent>()
                .Where(f => f.Page.Id == request.PageId && (regionIds.Contains(f.Region.Id) || pageContentIds.Contains(f.Id)))
                .ToFuture();

            // Demand access
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                var page = Repository
                    .AsQueryable<Root.Models.Page>()
                    .Where(p => p.Id == request.PageId)
                    .FetchMany(p => p.AccessRules)
                    .ToFuture()
                    .ToList()
                    .FirstOne();

                AccessControlService.DemandAccess(page, Context.Principal, AccessLevel.ReadWrite, RootModuleConstants.UserRoles.EditContent);
            }
            else
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            }

            var pageContents = contentsFuture.ToList();
            
            request.PageContents
                .GroupBy(group => new System.Tuple<Guid, Guid?>(group.RegionId, group.ParentPageContentId))
                .ForEach(group =>
                             {
                                 var regionId = group.Key.Item1;
                                 var parentPageContentId = group.Key.Item2;
                                 var region = Repository.AsProxy<Region>(regionId);
                                 var index = 0;

                                 foreach (var viewModel in group)
                                 {
                                     var pageContent = pageContents.FirstOrDefault(f => f.Id == viewModel.PageContentId);
                                     
                                     PageContent parentPageContent = null;
                                     if (parentPageContentId.HasValue && !parentPageContentId.Value.HasDefaultValue())
                                     {
                                         parentPageContent = pageContents.Where(f => f.Id == parentPageContentId).FirstOne();
                                     }

                                     if (pageContent == null)
                                     {
                                         throw new EntityNotFoundException(typeof(PageContent), Guid.Empty);
                                     }

                                     if (pageContent.Order != index 
                                         || pageContent.Region.Id != regionId
                                         || pageContent.Parent == null && parentPageContent != null
                                         || pageContent.Parent != null && parentPageContent == null
                                         || (pageContent.Parent != null && parentPageContent != null && parentPageContent.Id != pageContent.Parent.Id))
                                     {
                                         if (pageContent.Version != viewModel.Version)
                                         {
                                             throw new ConcurrentDataException(pageContent);
                                         }

                                         pageContent.Order = index;
                                         pageContent.Region = region;
                                         pageContent.Parent = parentPageContent;

                                         sortedPageContents.Add(pageContent);
                                         Repository.Save(pageContent);
                                     }
                                     index++;
                                 }
                             });

            UnitOfWork.Commit();

            // Notify
            foreach (var pageContent in sortedPageContents)
            {
                // Notify.
                Events.PageEvents.Instance.OnPageContentSorted(pageContent);
            }

            // Update versions
            foreach (var pageContent in pageContents)
            {
                var responseContent = response.PageContents.FirstOrDefault(pc => pc.PageContentId == pageContent.Id);
                if (responseContent != null)
                {
                    responseContent.Version = pageContent.Version;
                }
            }

            return response;
        }
    }
}