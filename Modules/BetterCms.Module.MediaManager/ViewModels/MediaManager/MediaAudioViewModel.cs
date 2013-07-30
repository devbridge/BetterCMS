using System;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaAudioViewModel : MediaFileViewModel
    {
        public MediaAudioViewModel()
        {
            Type = MediaType.Audio;            
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}