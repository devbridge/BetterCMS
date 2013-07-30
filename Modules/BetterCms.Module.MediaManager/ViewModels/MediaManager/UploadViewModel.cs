using System;
using System.Collections.Generic;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class UploadViewModel
    {
        public Guid CurrentFolderId { get; set; }

        public IDictionary<Guid, string> Folders { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("CurrentFolderId: {0}", CurrentFolderId);
        }
    }
}