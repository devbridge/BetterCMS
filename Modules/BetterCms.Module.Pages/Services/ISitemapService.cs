using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Services
{
    /// <summary>
    /// Sitemap service.
    /// </summary>
    public interface ISitemapService
    {
        /// <summary>
        /// Gets the node count.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Node count.</returns>
        int NodesWithUrl(string url);

        /// <summary>
        /// Changes the URL.
        /// </summary>
        /// <param name="oldUrl">The old URL.</param>
        /// <param name="newUrl">The new URL.</param>
        /// <returns>Node with new url count.</returns>
        int ChangeUrl(string oldUrl, string newUrl);

        /// <summary>
        /// Deletes the node.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        void DeleteNode(Guid id, int version, out IList<SitemapNode> deletedNodes);

        /// <summary>
        /// Deletes the node without page update.
        /// </summary>
        /// <param name="node">The node.</param>
        void DeleteNodeWithoutPageUpdate(SitemapNode node);

        /// <summary>
        /// Gets the nodes by URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Node list.</returns>
        IList<SitemapNode> GetNodesByUrl(string url);

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
        SitemapNode SaveNode(Guid id, int version, string url, string title, int displayOrder, Guid parentId, bool isDeleted = false, SitemapNode parentNode = null);
    }
}