using System.Collections.Generic;
using System.Linq;

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
                var sitemapNode = node.Id.HasDefaultValue()
                    ? new SitemapNode()
                    : Repository.First<SitemapNode>(node.Id);

                var oldUrl = sitemapNode.Url;
                var newUrl = node.Url;

                sitemapNode.IsDeleted = node.IsDeleted || (parentNode != null && parentNode.IsDeleted);

                if (sitemapNode.IsDeleted)
                {
                    if (!sitemapNode.Id.HasDefaultValue())
                    {
                        Repository.Delete(sitemapNode);
                    }
                }
                else
                {
                    sitemapNode.Version = node.Version;
                    sitemapNode.Title = node.Title;
                    sitemapNode.Url = node.Url;
                    sitemapNode.DisplayOrder = node.DisplayOrder;
                    if (parentNode != null && !parentNode.Id.HasDefaultValue())
                    {
                        sitemapNode.ParentNode = parentNode;
                    }

                    Repository.Save(sitemapNode);
                }

                SitemapService.UpdatedPageProperties(node.Id.HasDefaultValue(), sitemapNode.IsDeleted, oldUrl, newUrl);

                SaveNodeList(node.ChildNodes, sitemapNode);
            }
        }
   }
}