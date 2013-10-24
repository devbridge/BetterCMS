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
        /// Gets or sets the parent folder id.
        /// </summary>
        /// <value>
        /// The parent folder id.
        /// </value>
        public virtual Guid? ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the folder title.
        /// </summary>
        /// <value>
        /// The folder title.
        /// </value>
        public virtual string FolderTitle { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("FolderId: {0}, FolderTitle: {1}", FolderId, FolderTitle);
        }
    }
}