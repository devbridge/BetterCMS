using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic page properties.
    /// </summary>
    public interface IPage : IEntity
    {
        /// <summary>
        /// Gets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        PageStatus Status { get; }

        /// <summary>
        /// Gets a value indicating whether this page has SEO meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page has SEO; otherwise, <c>false</c>.
        /// </value>
        bool HasSEO { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }

        /// <summary>
        /// Gets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        string PageUrl { get; }

        /// <summary>
        /// Gets a value indicating whether page is master page.
        /// </summary>
        /// <value>
        /// <c>true</c> if page is master page; otherwise, <c>false</c>.
        /// </value>
        bool IsMasterPage { get; }
    }
}
