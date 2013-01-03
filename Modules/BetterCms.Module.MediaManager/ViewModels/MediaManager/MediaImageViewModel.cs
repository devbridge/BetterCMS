using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaImageViewModel : MediaFileViewModel
    {
        public string ThumbnailUrl { get; set; }
        
        public string PreviewUrl { get; set; }

        public string Tooltip { get; set; }

        public MediaImageViewModel()
        {
            Type = MediaType.Image;
        }
    }
}