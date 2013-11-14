using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaImageViewModel : MediaFileViewModel
    {
        public virtual int Width { get; set; }
        
        public virtual int Height { get; set; }

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