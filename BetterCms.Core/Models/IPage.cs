using System;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access basic page properties.
    /// </summary>
    public interface IPage
    {
        /// <summary>
        /// Gets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        Guid Id { get; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this page is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if this page is published; otherwise, <c>false</c>.
        /// </value>
        bool IsPublished { get; }

        /// <summary>
        /// Gets a value indicating whether this page has SEO meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page has SEO; otherwise, <c>false</c>.
        /// </value>
        bool HasSEO { get; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }

        string PageUrl { get; }
    }
}
