using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;

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
        public int ChangeUrl(string oldUrl, string newUrl)
        {
            oldUrl = (oldUrl ?? string.Empty).ToLower();

            if (string.IsNullOrEmpty(oldUrl) || string.IsNullOrEmpty(newUrl))
            {
                return 0;
            }

            var nodes = repository.AsQueryable<SitemapNode>(n => n.Url.ToLower() == oldUrl);
            var count = 0;
            foreach (var node in nodes)
            {
                node.Url = newUrl;
                repository.Save(node);
                count++;
            }

            return count;
        }

        /// <summary>
        /// Deletes the node.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        public void DeleteNode(Guid id, int version)
        {
            var node = repository.First<SitemapNode>(id);
            node.Version = version;

            repository.Delete(node);

            UpdatedPageProperties(id.HasDefaultValue(), true, node.Url, string.Empty);
        }

        /// <summary>
        /// Saves the node.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="displayOrder">The display order.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="parentNode">The parent node.</param>
        /// <returns>
        /// Updated or newly created sitemap node.
        /// </returns>
        public SitemapNode SaveNode(Guid id, int version, string url, string title, int displayOrder, Guid parentId, bool isDeleted = false, SitemapNode parentNode = null)
        {
            var node = id.HasDefaultValue()
                ? new SitemapNode()
                : repository.First<SitemapNode>(id);

            var oldUrl = node.Url;

            if (isDeleted)
            {
                if (!node.Id.HasDefaultValue())
                {
                    repository.Delete(node);
                    UpdatedPageProperties(id.HasDefaultValue(), node.IsDeleted, oldUrl, url);
                }
            }
            else
            {
                node.Version = version;
                node.Title = title;
                node.Url = url;
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
                UpdatedPageProperties(id.HasDefaultValue(), node.IsDeleted, oldUrl, url);
            }

            return node;
        }

        /// <summary>
        /// Gets the root nodes.
        /// </summary>
        /// <param name="search">The search query.</param>
        /// <returns>
        /// Sitemap node list.
        /// </returns>
        public IList<SitemapNode> GetRootNodes(string search)
        {
            var queryableNodes = repository.AsQueryable<SitemapNode>();

            if (!string.IsNullOrEmpty(search))
            {
                queryableNodes = queryableNodes.Where(n => n.Title.ToLower().Contains(search.ToLower()) || n.Url.ToLower().Contains(search.ToLower()));
                var nodes = queryableNodes.ToList();
                var rootNodes = new List<SitemapNode>();

                foreach (var node in nodes)
                {
                    var rootNode = GetRootNode(node);
                    if (!rootNodes.Contains(rootNode))
                    {
                        rootNodes.Add(rootNode);
                    }
                }

                return nodes;
            }

            queryableNodes = queryableNodes.Where(n => n.ParentNode == null);

            return queryableNodes.ToList();
        }

        /// <summary>
        /// Gets the root node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>Node that has no parent.</returns>
        private SitemapNode GetRootNode(SitemapNode node)
        {
            if (node.ParentNode != null)
            {
                return GetRootNode(node.ParentNode);
            }

            return node;
        }

        /// <summary>
        /// Updates page properties.
        /// </summary>
        /// <param name="isNodeNew">if set to <c>true</c> [is node new].</param>
        /// <param name="isNodeDeleted">if set to <c>true</c> [is node deleted].</param>
        /// <param name="oldUrl">The old URL.</param>
        /// <param name="newUrl">The new URL.</param>
        private void UpdatedPageProperties(bool isNodeNew, bool isNodeDeleted, string oldUrl, string newUrl)
        {
            oldUrl = (oldUrl ?? string.Empty).ToUpper();
            newUrl = (newUrl ?? string.Empty).ToUpper();

            if (isNodeNew)
            {
                if (string.IsNullOrEmpty(newUrl))
                {
                    return;
                }

                // New sitemap node created.
                if (!isNodeDeleted)
                {
                    var page = repository.FirstOrDefault<PageProperties>(p => p.PageUrl.ToUpper() == newUrl);
                    if (page != null)
                    {
                        page.NodeCountInSitemap += 1;
                        repository.Save(page);
                    }
                }
            }
            else if (isNodeDeleted)
            {
                if (string.IsNullOrEmpty(oldUrl))
                {
                    return;
                }

                // Sitemap node deleted.
                var page = repository.FirstOrDefault<PageProperties>(p => p.PageUrl.ToUpper() == oldUrl);
                if (page != null && page.NodeCountInSitemap > 0)
                {
                    page.NodeCountInSitemap -= 1;
                    repository.Save(page);
                }
            }
            else if (oldUrl != newUrl)
            {
                // Url in sitemap node changed.
                var pages = repository.AsQueryable<PageProperties>(p => p.PageUrl.ToUpper() == newUrl || p.PageUrl.ToUpper() == oldUrl).ToList();
                foreach (var page in pages)
                {
                    if (page.PageUrl == oldUrl && page.NodeCountInSitemap > 0)
                    {
                        page.NodeCountInSitemap -= 1;
                        repository.Save(page);
                    }
                    else if (page.PageUrl == newUrl)
                    {
                        page.NodeCountInSitemap += 1;
                        repository.Save(page);
                    }
                }
            }
        }
    }
}