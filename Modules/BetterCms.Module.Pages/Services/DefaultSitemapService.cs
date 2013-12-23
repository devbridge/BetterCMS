using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Services
{
    /// <summary>
    /// Default sitemap service.
    /// </summary>
    public class DefaultSitemapService : ISitemapService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSitemapService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultSitemapService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the node count.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Node count.</returns>
        public int NodesWithUrl(string url)
        {
            url = (url ?? string.Empty).ToLower();

            if (string.IsNullOrEmpty(url))
            {
                return 0;
            }

            return repository.AsQueryable<SitemapNode>(n => n.Url.ToLower() == url).Count();
        }

        /// <summary>
        /// Changes the URL.
        /// </summary>
        /// <param name="oldUrl">The old URL.</param>
        /// <param name="newUrl">The new URL.</param>
        /// <returns>
        /// Node with new url count.
        /// </returns>
        public void ChangeUrlsInAllSitemapsNodes(string oldUrl, string newUrl)
        {
            var oldUrlHash = (oldUrl ?? string.Empty).UrlHash();
            newUrl = newUrl ?? string.Empty;

            var nodes = repository.AsQueryable<SitemapNode>().Where(n => n.UrlHash == oldUrlHash).ToList();
            foreach (var node in nodes)
            {
                node.Url = newUrl;
                node.UrlHash = newUrl.UrlHash();
                repository.Save(node);
            }
        }

        /// <summary>
        /// Deletes the node.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        public void DeleteNode(Guid id, int version, out IList<SitemapNode> deletedNodes)
        {
            deletedNodes = new List<SitemapNode>();

            var node = repository.First<SitemapNode>(id);
            node.Version = version;
            DeleteNode(node, ref deletedNodes);
        }

        /// <summary>
        /// Gets the nodes by URL.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        /// Node list.
        /// </returns>
        public IList<SitemapNode> GetNodesByPage(Page page)
        {
            var urlHash = page.PageUrl.UrlHash();
            return repository
                .AsQueryable<SitemapNode>()
                .Where(n => (n.Page != null && n.Page.Id == page.Id) || (n.UrlHash == urlHash))
                .Fetch(node => node.ChildNodes)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Saves the node.
        /// </summary>
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="nodeId">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="pageId"></param>
        /// <param name="displayOrder">The display order.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="isDeleted">if set to <c>true</c> node is deleted.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <returns>
        /// Updated or newly created sitemap node.
        /// </returns>
        public SitemapNode SaveNode(Sitemap sitemap, Guid nodeId, int version, string url, string title, Guid pageId, int displayOrder, Guid parentId, bool isDeleted = false, SitemapNode parentNode = null)
        {
            var node = nodeId.HasDefaultValue()
                ? new SitemapNode()
                : repository.First<SitemapNode>(nodeId);

            if (isDeleted)
            {
                if (!node.Id.HasDefaultValue())
                {
                    repository.Delete(node);
                }
            }
            else
            {
                node.Sitemap = sitemap;
                node.Version = version;
                node.Title = title;
                node.Page = !pageId.HasDefaultValue() ? repository.AsProxy<PageProperties>(pageId) : null;
                node.Url = node.Page != null ? null : url;
                node.UrlHash = node.Page != null ? null : url.UrlHash();
                node.DisplayOrder = displayOrder;
                if (parentNode != null && !parentNode.Id.HasDefaultValue())
                {
                    node.ParentNode = parentNode;
                }
                else
                {
                    node.ParentNode = parentId.HasDefaultValue()
                        ? null
                        : repository.First<SitemapNode>(parentId);
                }

                repository.Save(node);
            }

            return node;
        }

        /// <summary>
        /// Deletes the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        public void DeleteNode(SitemapNode node, ref IList<SitemapNode> deletedNodes)
        {
            foreach (var childNode in node.ChildNodes)
            {
                DeleteNode(childNode, ref deletedNodes);
            }

            repository.Delete(node);
            deletedNodes.Add(node);
        }
    }
}