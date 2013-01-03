using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaAudioViewModel : MediaViewModel
    {
        public MediaAudioViewModel()
        {
            Type = MediaType.Audio;            
        }
    }
}