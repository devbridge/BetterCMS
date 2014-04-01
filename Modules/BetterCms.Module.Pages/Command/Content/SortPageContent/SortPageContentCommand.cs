using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.SortPageContent
{
    public class SortPageContentCommand : CommandBase, ICommand<PageContentSortViewModel, bool>
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
        public bool Execute(PageContentSortViewModel request)
        {
            UnitOfWork.BeginTransaction();

            var sortedPageContents = new List<PageContent>();
            var regionIds = request.PageContents.Select(r => r.RegionId).Distinct().ToArray();
            var pageContentIds = request.PageContents.Select(r => r.PageContentId).Distinct().ToArray();

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
                    .FetchMany(p => p.AccessRules)
                    .ToFuture()
                    .FirstOne();

                AccessControlService.DemandAccess(page, Context.Principal, AccessLevel.ReadWrite, RootModuleConstants.UserRoles.EditContent);
            }
            else
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            }

            var pageContents = contentsFuture.ToList();
            
            request.PageContents
                .GroupBy(group => group.RegionId)
                .ForEach(group =>
                             {
                                 var regionId = group.Key;
                                 var region = Repository.AsProxy<Region>(regionId);
                                 var index = 0;

                                 foreach (var viewModel in group)
                                 {
                                     var pageContent = pageContents.FirstOrDefault(f => f.Id == viewModel.PageContentId);

                                     if (pageContent == null)
                                     {
                                         throw new EntityNotFoundException(typeof(PageContent), Guid.Empty);
                                     }

                                     if (pageContent.Order != index || pageContent.Region.Id != regionId)
                                     {
                                         if (pageContent.Version != viewModel.Version)
                                         {
                                             throw new ConcurrentDataException(pageContent);
                                         }

                                         pageContent.Order = index;
                                         pageContent.Region = region;

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

            return true;
        }
    }
}