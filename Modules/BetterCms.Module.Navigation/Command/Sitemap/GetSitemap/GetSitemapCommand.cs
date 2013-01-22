using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Navigation.Models;
using BetterCms.Module.Navigation.Services;
using BetterCms.Module.Navigation.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Navigation.Command.Sitemap.GetSitemap
{
    /// <summary>
    /// Command to get sitemap data.
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

            return new SearchableSitemapViewModel
                {
                    SearchQuery = request,
                    RootNodes = GetSitemapNodesInHierarchy(rootNodes)
                };
        }

        /// <summary>
        /// Gets the sitemap nodes.
        /// </summary>
        /// <param name="sitemapNodes">The sitemap nodes.</param>
        /// <returns>A list of <see cref="SitemapNodeViewModel"/>.</returns>
        private IList<SitemapNodeViewModel> GetSitemapNodesInHierarchy(IList<SitemapNode> sitemapNodes)
        {
            var nodeList = new List<SitemapNodeViewModel>();

            foreach (var node in sitemapNodes)
            {
                nodeList.Add(new SitemapNodeViewModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        Title = node.Title,
                        Url = node.Url,
                        DisplayOrder = node.DisplayOrder,
                        ChildNodes = GetSitemapNodesInHierarchy(node.ChildNodes)
                    });
            }

            return nodeList.OrderBy(n => n.DisplayOrder).ToList();
        }
    }
}