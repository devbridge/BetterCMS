using System;

using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

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

        /// <summary>
        /// Gets or sets a value indicating whether use page title as node title.
        /// </summary>
        /// <value>
        /// <c>true</c> if use page title as node title; otherwise, <c>false</c>.
        /// </value>
        public virtual bool UsePageTitleAsNodeTitle { get; set; }

        /// <summary>
        /// Gets or sets the macro.
        /// </summary>
        /// <value>
        /// The macro.
        /// </value>
        public virtual string Macro { get; set; }
    }
}