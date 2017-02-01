using System.Collections.Generic;
using System.Globalization;

using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.ViewModels.Sitemap
{
    /// <summary>
    /// View model for sitemap and page links data.
    /// </summary>
    public class SitemapAndPageLinksViewModel
    {
        /// <summary>
        /// Gets or sets the page links.
        /// </summary>
        /// <value>
        /// The page links.
        /// </value>
        public SearchableGridViewModel<PageLinkViewModel> PageLinks { get; set; }

        /// <summary>
        /// Gets or sets the sitemap.
        /// </summary>
        /// <value>
        /// The sitemap.
        /// </value>
        public SitemapViewModel Sitemap { get; set; }

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
                PageLinks != null ? PageLinks.TotalCount.ToString(CultureInfo.InvariantCulture) : string.Empty,
                Sitemap != null && Sitemap.RootNodes != null ? Sitemap.RootNodes.Count.ToString(CultureInfo.InvariantCulture) : string.Empty);
        }
    }
}