using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaFolderViewModel : MediaViewModel
    {
        public MediaFolderViewModel()
        {
            ContentType = MediaContentType.Folder;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}