using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaPathViewModel
    {
        public MediaType MediaType { get; set; }

        public MediaFolderViewModel CurrentFolder { get; set; }

        public IEnumerable<MediaFolderViewModel> Folders { get; set; }        
    }
}