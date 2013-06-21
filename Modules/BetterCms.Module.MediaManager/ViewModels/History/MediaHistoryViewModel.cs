using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.MediaManager.ViewModels.History
{
    /// <summary>
    /// Media history view model.
    /// </summary>
    public class MediaHistoryViewModel : SearchableGridViewModel<MediaHistoryItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaHistoryViewModel"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="options">The options.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="mediaId">The media id.</param>
        public MediaHistoryViewModel(IEnumerable<MediaHistoryItem> items, SearchableGridOptions options, int totalCount, Guid mediaId)
            : base(items, options, totalCount)
        {
            MediaId = mediaId;
        }

        /// <summary>
        /// Gets or sets the media id.
        /// </summary>
        /// <value>
        /// The media id.
        /// </value>
        public Guid MediaId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("ContentId: {0}", MediaId);
        }
    }
}