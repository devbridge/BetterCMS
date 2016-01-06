using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.GetSitemapHistory
{
    /// <summary>
    /// Command to load a list of the sitemap history versions.
    /// </summary>
    public class GetSitemapHistoryCommand : CommandBase, ICommand<GetSitemapHistoryRequest, SitemapHistoryViewModel>
    {
        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSitemapHistoryCommand" /> class.
        /// </summary>
        /// <param name="sitemapService">The sitemap service.</param>
        public GetSitemapHistoryCommand(ISitemapService sitemapService, ICmsConfiguration cmsConfiguration)
        {
            this.sitemapService = sitemapService;
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The view model with list of history view models.</returns>
        public SitemapHistoryViewModel Execute(GetSitemapHistoryRequest request)
        {
            var currentVersionQuery = Repository.AsQueryable<Models.Sitemap>()
                      .Where(map => map.Id == request.SitemapId);

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                currentVersionQuery = currentVersionQuery.FetchMany(f => f.AccessRules);
            }

            var currentVersion = currentVersionQuery.Distinct().ToList().First();

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                AccessControlService.DemandAccess(currentVersion, Context.Principal, AccessLevel.Read);
            }

            var history = new List<SitemapHistoryItem>
                {
                    new SitemapHistoryItem
                        {
                            Id = currentVersion.Id,
                            Version = currentVersion.Version,
                            SitemapTitle = currentVersion.Title,
                            StatusName = NavigationGlobalization.SitemapStatus_Active,
                            ArchivedOn = null,
                            ArchivedByUser = null,
                            CanCurrentUserRestoreIt = false
                        }
                };

            var historyEntities = sitemapService.GetSitemapHistory(request.SitemapId);
            history.AddRange(
                historyEntities.Select(
                    archive =>
                    new SitemapHistoryItem
                        {
                            Id = archive.Id,
                            Version = archive.Version,
                            SitemapTitle = archive.Title,
                            StatusName = NavigationGlobalization.SitemapStatus_Archived,
                            ArchivedOn = archive.CreatedOn,
                            ArchivedByUser = archive.CreatedByUser,
                            CanCurrentUserRestoreIt = true
                        }).ToList());

            history = AddSortAndPaging(history, request);

            return new SitemapHistoryViewModel(history, request, history.Count, request.SitemapId);
        }

        /// <summary>
        /// Adds the sort and paging.
        /// </summary>
        /// <param name="history">The history.</param>
        /// <param name="gridOptions">The grid options.</param>
        /// <returns>Sorted list.</returns>
        private static List<SitemapHistoryItem> AddSortAndPaging(List<SitemapHistoryItem> history, Root.Mvc.Grids.GridOptions.SearchableGridOptions gridOptions)
        {
            if (string.IsNullOrWhiteSpace(gridOptions.Column))
            {
                history = history.AsQueryable().OrderBy(o => o.CanCurrentUserRestoreIt).ThenByDescending(o => o.ArchivedOn).AddPaging(gridOptions).ToList();
            }
            else
            {
                history = history.AsQueryable().AddSortingAndPaging(gridOptions).ToList();
            }

            return history;
        }
    }
}