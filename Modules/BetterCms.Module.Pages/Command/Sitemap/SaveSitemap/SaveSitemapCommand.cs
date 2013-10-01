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
    public class SaveSitemapCommand : CommandBase, ICommandIn<IList<SitemapNodeViewModel>>
    {
        private IList<SitemapNode> createdNodes = new List<SitemapNode>();
        private IList<SitemapNode> updatedNodes = new List<SitemapNode>();
        private IList<SitemapNode> deletedNodes = new List<SitemapNode>();

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
            createdNodes.Clear();
            updatedNodes.Clear();
            deletedNodes.Clear();

            UnitOfWork.BeginTransaction();
            SaveNodeList(request, null);
            UnitOfWork.Commit();

            if (createdNodes.Count <= 0 && updatedNodes.Count <= 0 && deletedNodes.Count <= 0)
            {
                return;
            }

            foreach (var node in createdNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeCreated(node);
            }

            foreach (var node in updatedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
            }

            foreach (var node in deletedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeDeleted(node);
            }

            Events.SitemapEvents.Instance.OnSitemapUpdated();
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
                var isDeleted = node.IsDeleted || (parentNode != null && parentNode.IsDeleted);
                var create = node.Id.HasDefaultValue() && !isDeleted;
                var update = !node.Id.HasDefaultValue() && !isDeleted;
                var delete = !node.Id.HasDefaultValue() && isDeleted;

                var sitemapNode = SitemapService.SaveNode(node.Id, node.Version, node.Url, node.Title, node.DisplayOrder, node.ParentId, isDeleted, parentNode);

                if (create)
                {
                    createdNodes.Add(sitemapNode);
                }
                else if (update)
                {
                    updatedNodes.Add(sitemapNode);
                }
                else if (delete)
                {
                    deletedNodes.Add(sitemapNode);
                }

                SaveNodeList(node.ChildNodes, sitemapNode);
            }
        }
   }
}