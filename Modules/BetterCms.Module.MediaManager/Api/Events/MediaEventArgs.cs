using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Api.Events
{
    /// <summary>
    /// Page Created Event Arguments
    /// </summary>
    public class MediaEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaEventArgs" /> class.
        /// </summary>
        /// <param name="media">A media object.</param>
        public MediaEventArgs(Media media)
        {
            Media = media;
        }

        /// <summary>
        /// Gets or sets the created page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public Media Media { get; set; }
    }
}
