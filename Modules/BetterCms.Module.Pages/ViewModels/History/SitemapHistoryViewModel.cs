using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.ViewModels.History
{
    /// <summary>
    /// Sitemap history view model.
    /// </summary>
    public class SitemapHistoryViewModel : SearchableGridViewModel<SitemapHistoryItem> 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapHistoryViewModel"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="options">The options.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="sitemapId">The sitemap identifier.</param>
        public SitemapHistoryViewModel(IEnumerable<SitemapHistoryItem> items, SearchableGridOptions options, int totalCount, Guid sitemapId)
            : base(items, options, totalCount)
        {
            SitemapId = sitemapId;
        }

        /// <summary>
        /// Gets or sets the sitemap identifier.
        /// </summary>
        /// <value>
        /// The sitemap identifier.
        /// </value>
        public Guid SitemapId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("SitemapId: {0}", SitemapId);
        }
    }
}