using System;

namespace BetterCms.Module.MediaManager.ViewModels
{
    public class FolderSelectorViewModel
    {
        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public virtual Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the folder title.
        /// </summary>
        /// <value>
        /// The folder title.
        /// </value>
        public virtual string FolderTitle { get; set; }
    }
}