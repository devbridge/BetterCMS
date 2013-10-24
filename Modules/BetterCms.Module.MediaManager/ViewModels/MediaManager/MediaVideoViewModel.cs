using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaVideoViewModel : MediaFileViewModel
    {
        public MediaVideoViewModel()
        {
            Type = MediaType.Video;            
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}