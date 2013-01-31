using System.Collections.Generic;

namespace BetterCms.Module.Navigation.ViewModels.Sitemap
{
    /// <summary>
    /// View model for sitemap data.
    /// </summary>
    public class SearchableSitemapViewModel
    {
        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>
        /// The search query.
        /// </value>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Gets or sets the sitemap root nodes.
        /// </summary>
        /// <value>
        /// The root nodes.
        /// </value>
        public IList<SitemapNodeViewModel> RootNodes { get; set; }
    }
}