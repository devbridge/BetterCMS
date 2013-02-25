using System;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaVideoViewModel : MediaFileViewModel
    {
        public MediaVideoViewModel()
        {
            Type = MediaType.Video;            
        }
    }
}