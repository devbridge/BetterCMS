using System;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.History
{
    /// <summary>
    /// Sitemap history item view model.
    /// </summary>
    public class SitemapHistoryItem : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the archived on.
        /// </summary>
        /// <value>
        /// The archived on.
        /// </value>
        public DateTime? ArchivedOn { get; set; }

        /// <summary>
        /// Gets or sets the archived by user.
        /// </summary>
        /// <value>
        /// The archived by user.
        /// </value>
        public string ArchivedByUser { get; set; }

        /// <summary>
        /// Gets or sets the name of the status.
        /// </summary>
        /// <value>
        /// The name of the status.
        /// </value>
        public string StatusName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string SitemapTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current user can restore it.
        /// </summary>
        /// <value>
        /// <c>true</c> if current user can restore it; otherwise, <c>false</c>.
        /// </value>
        public bool CanCurrentUserRestoreIt { get; set; }
    }
}