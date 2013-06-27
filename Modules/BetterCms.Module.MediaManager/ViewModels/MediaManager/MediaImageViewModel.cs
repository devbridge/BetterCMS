using System;

using BetterCms.Core.DataContracts.Enums;
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
    }
}