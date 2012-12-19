using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    /// <summary>
    /// A generic page entity.
    /// </summary>
    [Serializable]
    public class Page : EquatableEntity<Page>, IPage
    {
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public virtual string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page is published.
        /// </summary>
        /// <value>
        ///   <c>true</c> if page is published; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsPublished { get; set; }

        public virtual DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets a value indicating whether this page has SEO meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page has SEO; otherwise, <c>false</c>.
        /// </value>
        bool IPage.HasSEO
        {
            get
            {
                return !string.IsNullOrWhiteSpace(MetaTitle) && !string.IsNullOrWhiteSpace(MetaKeywords) && !string.IsNullOrWhiteSpace(MetaDescription);
            }
        }

        /// <summary>
        /// Gets or sets the page meta title.
        /// </summary>
        /// <value>
        /// The page meta title.
        /// </value>
        public virtual string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the page meta keywords.
        /// </summary>
        /// <value>
        /// The page meta keywords.
        /// </value>
        public virtual string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the page meta description.
        /// </summary>
        /// <value>
        /// The page meta description.
        /// </value>
        public virtual string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the page layout.
        /// </summary>
        /// <value>
        /// The page layout.
        /// </value>
        public virtual Layout Layout { get; set; }

        /// <summary>
        /// Gets or sets the page contents.
        /// </summary>
        /// <value>
        /// The page contents.
        /// </value>
        public virtual IList<PageContent> PageContents { get; set; }
    }
}