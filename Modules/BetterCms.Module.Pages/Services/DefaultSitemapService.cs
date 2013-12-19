using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

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
        /// <param name="deletedNodes">The deleted nodes.</param>
        public void DeleteNode(Guid id, int version, out IList<SitemapNode> deletedNodes)
        {
            deletedNodes = new List<SitemapNode>();

            var node = repository.First<SitemapNode>(id);
            node.Version = version;
            DeleteNode(node, ref deletedNodes);
        }

        /// <summary>
        /// Deletes the node without page update.
        /// </summary>
        /// <param name="node">The node.</param>
        public void DeleteNodeWithoutPageUpdate(SitemapNode node)
        {
            repository.Delete(node);
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
            var url = page.PageUrl.ToLower();
            return repository
                .AsQueryable<SitemapNode>(n => (n.Page != null && n.Page.Id == page.Id) || (n.Url != null && n.Url.ToLower() == url))
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

            var oldUrl = node.Url;

            if (isDeleted)
            {
                if (!node.Id.HasDefaultValue())
                {
                    repository.Delete(node);
                    UpdatePageProperties(nodeId.HasDefaultValue(), node.IsDeleted, oldUrl, url, pageId);
                }
            }
            else
            {
                node.Sitemap = sitemap;
                node.Version = version;
                node.Title = title;
                node.Page = !pageId.HasDefaultValue() ? repository.AsProxy<PageProperties>(pageId) : null;
                node.Url = node.Page != null ? null : url;
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
                UpdatePageProperties(nodeId.HasDefaultValue(), node.IsDeleted, oldUrl, url, pageId);
            }

            return node;
        }

        /// <summary>
        /// Lowers NodeCountInSitemap property value for pages related with removedNodes.
        /// </summary>
        /// <param name="removedNodes">The removed nodes.</param>
        public void DecreaseNodeCountForPages(IList<SitemapNode> removedNodes)
        {
            foreach (var node in removedNodes)
            {
                PageProperties page;
                if (node.Page != null)
                {
                    page = node.Page;
                }
                else
                {
                    var hashedUrl = node.Url.UrlHash();
                    page = repository.AsQueryable<PageProperties>().FirstOrDefault(p => p.PageUrlHash == hashedUrl);
                }

                if (page != null)
                {
                    var value = page.NodeCountInSitemap - 1;
                    page.NodeCountInSitemap = value > 0 ? value : 0;
                    repository.Save(page);
                }
            }
        }

        /// <summary>
        /// Deletes the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        private void DeleteNode(SitemapNode node, ref IList<SitemapNode> deletedNodes)
        {
            foreach (var childNode in node.ChildNodes)
            {
                DeleteNode(childNode, ref deletedNodes);
            }

            repository.Delete(node);
            deletedNodes.Add(node);

            UpdatePageProperties(false, true, node.Url, string.Empty, node.Page != null ? node.Page.Id : Guid.Empty);
        }

        /// <summary>
        /// Updates page properties.
        /// </summary>
        /// <param name="isNodeNew">if set to <c>true</c> [is node new].</param>
        /// <param name="isNodeDeleted">if set to <c>true</c> [is node deleted].</param>
        /// <param name="oldUrl">The old URL.</param>
        /// <param name="newUrl">The new URL.</param>
        /// <param name="pageId">The page identifier.</param>
        private void UpdatePageProperties(bool isNodeNew, bool isNodeDeleted, string oldUrl, string newUrl, Guid pageId)
        {
            oldUrl = (oldUrl ?? string.Empty).ToLower();
            newUrl = (newUrl ?? string.Empty).ToLower();
            var getPageById = !pageId.HasDefaultValue();

            if (isNodeNew)
            {
                if (string.IsNullOrEmpty(newUrl))
                {
                    return;
                }

                // New sitemap node created.
                if (!isNodeDeleted)
                {
                    var page = repository.FirstOrDefault<PageProperties>(p => (getPageById && p.Id == pageId) || (!getPageById && p.PageUrl.ToLower() == newUrl));
                    if (page != null)
                    {
                        page.NodeCountInSitemap += 1;
                        page.SaveUnsecured = true;

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
                var page = repository.FirstOrDefault<PageProperties>(p => (getPageById && p.Id == pageId) || (!getPageById && p.PageUrl.ToLower() == oldUrl));
                if (page != null && page.NodeCountInSitemap > 0)
                {
                    page.NodeCountInSitemap -= 1;
                    page.SaveUnsecured = true;

                    repository.Save(page);
                }
            }
            else if (oldUrl != newUrl)
            {
                // Url in sitemap node changed.
                var pages = repository.AsQueryable<PageProperties>(p => p.PageUrl.ToLower() == newUrl || p.PageUrl.ToLower() == oldUrl).ToList();
                foreach (var page in pages)
                {
                    if (page.PageUrl.ToLower() == oldUrl && page.NodeCountInSitemap > 0)
                    {
                        page.NodeCountInSitemap -= 1;
                        page.SaveUnsecured = true;

                        repository.Save(page);
                    }
                    else if (page.PageUrl.ToLower() == newUrl)
                    {
                        page.NodeCountInSitemap += 1;
                        page.SaveUnsecured = true;

                        repository.Save(page);
                    }
                }
            }
        }
    }
}