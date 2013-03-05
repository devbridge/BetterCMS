using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Sitemap.SaveSitemap
{
    /// <summary>
    /// Saves sitemap data.
    /// </summary>
    public class SaveSitemapCommand : CommandBase, ICommand<IList<SitemapNodeViewModel>>
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
        public void Execute(IList<SitemapNodeViewModel> request)
        {
            UnitOfWork.BeginTransaction();
            SaveNodeList(request, null);
            UnitOfWork.Commit();
        }

        /// <summary>
        /// Saves the node list.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="parentNode">The parent node.</param>
        private void SaveNodeList(IEnumerable<SitemapNodeViewModel> nodes, SitemapNode parentNode)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (var node in nodes)
            {
                var sitemapNode = SitemapService.SaveNode(node.Id, node.Version, node.Url, node.Title, node.DisplayOrder, node.ParentId, node.IsDeleted || (parentNode != null && parentNode.IsDeleted), parentNode);

                SaveNodeList(node.ChildNodes, sitemapNode);
            }
        }
   }
}