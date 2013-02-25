using System;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaImageViewModel : MediaFileViewModel
    {
        public string ThumbnailUrl { get; set; }

        public string Tooltip { get; set; }

        public MediaImageViewModel()
        {
            Type = MediaType.Image;
        }
    }
}