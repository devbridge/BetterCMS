using System.Collections.Generic;

namespace BetterCms.Module.Navigation.ViewModels.Sitemap
{
    /// <summary>
    /// View model for sitemap and page links data.
    /// </summary>
    public class SitemapAndPageLinksViewModel
    {
        /// <summary>
        /// Gets or sets the sitemap root nodes.
        /// </summary>
        /// <value>
        /// The root nodes.
        /// </value>
        public IList<SitemapNodeViewModel> RootNodes { get; set; }

        /// <summary>
        /// Gets or sets the page links.
        /// </summary>
        /// <value>
        /// The page links.
        /// </value>
        public IList<PageLinkViewModel> PageLinks { get; set; }
    }
}