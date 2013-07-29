using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaImageViewModel : MediaFileViewModel
    {
        public MediaImageViewModel()
        {
            Type = MediaType.Image;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}