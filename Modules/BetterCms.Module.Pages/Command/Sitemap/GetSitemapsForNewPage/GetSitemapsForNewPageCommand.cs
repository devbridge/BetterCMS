using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.GetSitemapsForNewPage
{
    /// <summary>
    /// Command to get sitemap data.
    /// </summary>
    public class GetSitemapsForNewPageCommand : CommandBase, ICommandOut<List<SitemapViewModel>>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <returns>
        /// Sitemap list.
        /// </returns>
        public List<SitemapViewModel> Execute()
        {
            var sitemaps = new List<SitemapViewModel>();

            var allSitmaps = Repository.AsQueryable<Models.Sitemap>()
                .FetchMany(map => map.AccessRules)
                .FetchMany(map => map.Nodes)
                .ThenFetch(node => node.Page)
                .ToList();

            foreach (var sitemap in allSitmaps)
            {
                var model = new SitemapViewModel
                    {
                        Id = sitemap.Id,
                        Version = sitemap.Version,
                        Title = sitemap.Title,
                        RootNodes = GetSitemapNodesInHierarchy(sitemap.Nodes.Where(f => f.ParentNode == null).Distinct().ToList(), sitemap.Nodes.Distinct().ToList()),
                        AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled
                    };

                if (CmsConfiguration.Security.AccessControlEnabled)
                {
                    model.UserAccessList = sitemap.AccessRules.Distinct().Select(x => new UserAccessViewModel(x)).ToList();
                    var rules = model.UserAccessList.Cast<IAccessRule>().ToList();
                    SetIsReadOnly(model, rules);
                }

                if (!model.IsReadOnly)
                {
                    sitemaps.Add(model);
                }
            }

            return sitemaps.Count > 0 ? sitemaps : null;
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