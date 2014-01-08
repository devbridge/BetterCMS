using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.GetSitemapVersion
{
    /// <summary>
    /// Command to get sitemap data.
    /// </summary>
    public class GetSitemapVersionCommand : CommandBase, ICommand<Guid, SitemapViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

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
        /// <param name="versionId">The sitemap version identifier.</param>
        /// <returns>
        /// Sitemap view model.
        /// </returns>
        public SitemapViewModel Execute(Guid versionId)
        {
            // Return current version.
            var sitemap = Repository.AsQueryable<Models.Sitemap>()
                .Where(map => map.Id == versionId)
                .FetchMany(map => map.Nodes)
                .ThenFetch(node => node.Page)
                .Distinct()
                .ToList()
                .FirstOrDefault() ?? SitemapService.GetArchivedSitemapVersionForPreview(versionId);

            if (sitemap != null)
            {
                var model = new SitemapViewModel
                    {
                        Id = sitemap.Id,
                        Version = sitemap.Version,
                        Title = sitemap.Title,
                        RootNodes = GetSitemapNodesInHierarchy(sitemap.Nodes.Where(f => f.ParentNode == null).ToList(), sitemap.Nodes.ToList()),
                        AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled,
                        IsReadOnly = true
                    };

                return model;
            }

            return null;
        }

        /// <summary>
        /// Gets the sitemap nodes in hierarchy.
        /// </summary>
        /// <param name="sitemapNodes">The sitemap nodes.</param>
        /// <param name="allNodes">All nodes.</param>
        /// <returns>The list with all root nodes.</returns>
        private static List<SitemapNodeViewModel> GetSitemapNodesInHierarchy(IList<SitemapNode> sitemapNodes, IList<SitemapNode> allNodes)
        {
            var nodeList = new List<SitemapNodeViewModel>();

            foreach (var node in sitemapNodes)
            {
                nodeList.Add(new SitemapNodeViewModel
                {
                    Id = node.Id,
                    Version = node.Version,
                    Title = node.Title,
                    Url = node.Page != null ? node.Page.PageUrl : node.Url,
                    PageId = node.Page != null ? node.Page.Id : Guid.Empty,
                    DisplayOrder = node.DisplayOrder,
                    ChildNodes = GetSitemapNodesInHierarchy(allNodes.Where(f => f.ParentNode == node).ToList(), allNodes)
                });
            }

            return nodeList.OrderBy(n => n.DisplayOrder).ToList();
        }
    }
}