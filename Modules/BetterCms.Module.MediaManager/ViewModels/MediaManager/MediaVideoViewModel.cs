using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaVideoViewModel : MediaViewModel
    {
        public MediaVideoViewModel()
        {
            Type = MediaType.Video;            
        }
    }
}