using System;
using System.Collections.Generic;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaPathViewModel
    {
        public MediaFolderViewModel CurrentFolder { get; set; }

        public IEnumerable<MediaFolderViewModel> Folders { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("CurrentFolder: {0}", CurrentFolder);
        }
    }
}