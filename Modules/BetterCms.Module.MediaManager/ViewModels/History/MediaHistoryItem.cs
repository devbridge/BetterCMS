using System;

using BetterCms.Module.MediaManager.Models.Enum;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.MediaManager.ViewModels.History
{
    /// <summary>
    /// View model for media history item.
    /// </summary>
    public class MediaHistoryItem : IEditableGridItem
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
        /// Gets or sets a value indicating whether this instance can current user restore it.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can current user restore it; otherwise, <c>false</c>.
        /// </value>
        public bool CanCurrentUserRestoreIt { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        public DateTime PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the created by user.
        /// </summary>
        /// <value>
        /// The created by user.
        /// </value>
        public string PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets the archived on.
        /// </summary>
        /// <value>
        /// The archived on.
        /// </value>
        public DateTime? ArchivedOn { get; set; }

        /// <summary>
        /// Gets or sets the displayed for.
        /// </summary>
        /// <value>
        /// The displayed for.
        /// </value>
        public TimeSpan? DisplayedFor { get; set; }

        /// <summary>
        /// Gets or sets the name of the status.
        /// </summary>
        /// <value>
        /// The name of the status.
        /// </value>
        public string StatusName { get; set; }

        /// <summary>
        /// Gets or sets the media item status.
        /// </summary>
        /// <value>
        /// The media item status.
        /// </value>
        public MediaHistoryStatus Status { get; set; }
    }
}