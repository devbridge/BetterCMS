using System.Collections.Generic;
using System.Globalization;

namespace BetterCms.Module.Pages.ViewModels.Sitemap
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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("SearchQuery: {0}, RootNodes count: {1}", SearchQuery, RootNodes != null ? RootNodes.Count.ToString(CultureInfo.InvariantCulture) : string.Empty);
        }
    }
}