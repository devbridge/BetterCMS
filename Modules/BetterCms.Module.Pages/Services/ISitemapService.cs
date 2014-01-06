using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.Command.History.GetSitemapHistory;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Services
{
    /// <summary>
    /// Sitemap service.
    /// </summary>
    public interface ISitemapService
    {
        /// <summary>
        /// Changes the URL.
        /// </summary>
        /// <param name="oldUrl">The old URL.</param>
        /// <param name="newUrl">The new URL.</param>
        /// <returns>Updated nodes.</returns>
        IList<SitemapNode> ChangeUrlsInAllSitemapsNodes(string oldUrl, string newUrl);

        /// <summary>
        /// Deletes the node and child nodes.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        void DeleteNode(Guid id, int version, out IList<SitemapNode> deletedNodes);

        /// <summary>
        /// Deletes the node and child nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        void DeleteNode(SitemapNode node, ref IList<SitemapNode> deletedNodes);

        /// <summary>
        /// Gets the nodes by URL.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        /// Node list.
        /// </returns>
        IList<SitemapNode> GetNodesByPage(Page page);

        /// <summary>
        /// Saves the node.
        /// </summary>
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="nodeId">The node identifier.</param>
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
        SitemapNode SaveNode(Sitemap sitemap, Guid nodeId, int version, string url, string title, Guid pageId, int displayOrder, Guid parentId, bool isDeleted = false, SitemapNode parentNode = null);

        /// <summary>
        /// Gets the sitemap history.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns>Sitemap previous archived versions.</returns>
        IList<SitemapArchive> GetSitemapHistory(Guid sitemapId);

        /// <summary>
        /// Archives the sitemap.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        void ArchiveSitemap(Guid sitemapId);
    }
}