using System;

namespace BetterCms.Module.MediaManager.Command.MediaManager.UnarchiveMedia
{
    public class UnarchiveMediaCommandRequest
    {
        /// <summary>
        /// Gets or sets the media id.
        /// </summary>
        /// <value>
        /// The media id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public int Version { get; set; }
    }
}