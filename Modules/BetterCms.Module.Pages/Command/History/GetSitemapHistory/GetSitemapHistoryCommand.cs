using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Command.History.GetContentHistory;
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
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContentHistoryCommand" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public GetSitemapHistoryCommand(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The view model with list of history view models.</returns>
        public SitemapHistoryViewModel Execute(GetSitemapHistoryRequest request)
        {
            // TODO: implement.
            return new SitemapHistoryViewModel(new List<SitemapHistoryItem>(), request, 0, request.SitemapId);
        }
    }
}