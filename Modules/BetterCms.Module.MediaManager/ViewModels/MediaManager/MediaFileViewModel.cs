using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaFileViewModel : MediaViewModel
    {
        public virtual long Size { get; set; }

        public virtual string FileExtension { get; set; }

        public virtual string PublicUrl { get; set; }

        public virtual bool IsProcessing { get; set; }
        
        public virtual bool IsFailed { get; set; }

        public MediaFileViewModel()
        {
            Type = MediaType.File;
        }
    }
}