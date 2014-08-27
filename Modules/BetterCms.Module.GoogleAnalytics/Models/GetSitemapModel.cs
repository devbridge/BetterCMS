using System;

namespace BetterCms.Module.GoogleAnalytics.Models
{
    public class GetSitemapModel
    {
        /// <summary>
        /// Gets or sets the sitemap identifier.
        /// </summary>
        /// <value>
        /// The sitemap identifier.
        /// </value>
        public Guid SitemapId { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        public Guid LanguageId { get; set; }
    }
}