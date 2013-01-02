using System;
using System.Collections.Generic;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaPathViewModel
    {
        public MediaFolderViewModel CurrentFolder { get; set; }

        public IEnumerable<MediaFolderViewModel> Folders { get; set; }        
    }
}