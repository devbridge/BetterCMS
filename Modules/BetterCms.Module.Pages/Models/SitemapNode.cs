using System;
using System.Collections.Generic;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    /// <summary>
    /// Sitemap node class.
    /// </summary>
    [Serializable]
    public class SitemapNode : EquatableEntity<SitemapNode>
    {
        /// <summary>
        /// Gets or sets the sitemap.
        /// </summary>
        /// <value>
        /// The sitemap.
        /// </value>
        public virtual Sitemap Sitemap { get; set; }

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
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public virtual Page Page { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>
        /// The display order.
        /// </value>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public virtual SitemapNode ParentNode { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public virtual IList<SitemapNode> ChildNodes { get; set; }
    }
}