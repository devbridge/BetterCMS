using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaFileViewModel : MediaViewModel
    {
        public virtual long Size { get; set; }

        public MediaFileViewModel()
        {
            Type = MediaType.File;
        }
    }
}