using System;

namespace BetterCms.Module.MediaManager.Command.Folder
{
    /// <summary>
    /// Data contract for DeleteFolderCommand.
    /// </summary>
    public class DeleteFolderCommandRequest
    {
        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public Guid FolderId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}