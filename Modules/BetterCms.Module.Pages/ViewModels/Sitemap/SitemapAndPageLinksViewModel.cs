using System.Collections.Generic;
using System.Globalization;

namespace BetterCms.Module.Pages.ViewModels.Sitemap
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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "PageLinks count: {0}, RootNodes count: {1}",
                PageLinks != null ? PageLinks.Count.ToString(CultureInfo.InvariantCulture) : string.Empty,
                RootNodes != null ? RootNodes.Count.ToString(CultureInfo.InvariantCulture) : string.Empty);
        }
    }
}