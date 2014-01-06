using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.History.GetSitemapHistory
{
    /// <summary>
    /// Command to load a list of the content history versions.
    /// </summary>
    public class GetSitemapHistoryCommand : CommandBase, ICommand<GetSitemapHistoryRequest, SitemapHistoryViewModel>
    {
        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSitemapHistoryCommand" /> class.
        /// </summary>
        /// <param name="sitemapService">The sitemap service.</param>
        public GetSitemapHistoryCommand(ISitemapService sitemapService)
        {
            this.sitemapService = sitemapService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The view model with list of history view models.</returns>
        public SitemapHistoryViewModel Execute(GetSitemapHistoryRequest request)
        {
            var historyEntities = sitemapService.GetSitemapHistory(request.SitemapId);
            var history =
                historyEntities.Select(
                    archive =>
                    new SitemapHistoryItem
                        {
                            Id = archive.Id,
                            Version = archive.Version,
                            StatusName = PagesGlobalization.ContentStatus_Archived, // TODO: move to navigation globalization.
                            ArchivedByUser = archive.CreatedByUser,
                            ArchivedOn = archive.CreatedOn,
                            DisplayedFor = (TimeSpan?)null,
                            PublishedByUser = null,
                            PublishedOn = null,
                            CanCurrentUserRestoreIt = true
                        }).ToList();

            // TODO: recalculate DisplayedFor field.

            return new SitemapHistoryViewModel(history, request, 0, request.SitemapId);
        }
    }
}