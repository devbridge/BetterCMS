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
        /// Gets the root nodes.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns>Sitemap node list.</returns>
        IList<SitemapNode> GetRootNodes(string search);

        /// <summary>
        /// Updates page properties.
        /// </summary>
        /// <param name="isNodeNew">if set to <c>true</c> [is node new].</param>
        /// <param name="isNodeDeleted">if set to <c>true</c> [is node deleted].</param>
        /// <param name="oldUrl">The old URL.</param>
        /// <param name="newUrl">The new URL.</param>
        void UpdatedPageProperties(bool isNodeNew, bool isNodeDeleted, string oldUrl, string newUrl);
    }
}