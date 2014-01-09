using System;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    /// <summary>
    /// Sitemap node translation class.
    /// </summary>
    [Serializable]
    public class SitemapNodeTranslation : EquatableEntity<SitemapNodeTranslation>
    {
        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public virtual SitemapNode Node { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public virtual Language Language { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public virtual string Url { get; set; }

        /// <summary>
        /// Gets or sets the URL hash.
        /// </summary>
        /// <value>
        /// The URL hash.
        /// </value>
        public virtual string UrlHash { get; set; }
    }
}