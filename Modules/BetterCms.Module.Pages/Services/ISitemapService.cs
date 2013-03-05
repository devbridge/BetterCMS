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
    }
}