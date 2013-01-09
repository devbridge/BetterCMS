using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Navigation.Services;
using BetterCms.Module.Navigation.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Navigation.Command.Sitemap.GetSitemap
{
    /// <summary>
    /// Command to get media image data.
    /// </summary>
    public class GetSitemapCommand : CommandBase, ICommand<string, SearchableSitemapViewModel>
    {
        /// <summary>
        /// Gets or sets the sitemap service.
        /// </summary>
        /// <value>
        /// The sitemap service.
        /// </value>
        public ISitemapService SitemapService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Sitemap root nodes.</returns>
        public SearchableSitemapViewModel Execute(string request)
        {
            var rootNodes = SitemapService.GetRootNodes(request);

            // TODO: implement.

            return new SearchableSitemapViewModel
                {
                    SearchQuery = request, 
                    RootNodes = new List<SitemapNodeViewModel>()
                };
        }
    }
}