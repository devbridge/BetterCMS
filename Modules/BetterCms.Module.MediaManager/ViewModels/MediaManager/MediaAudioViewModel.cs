using System;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaAudioViewModel : MediaFileViewModel
    {
        public MediaAudioViewModel()
        {
            Type = MediaType.Audio;            
        }
    }
}