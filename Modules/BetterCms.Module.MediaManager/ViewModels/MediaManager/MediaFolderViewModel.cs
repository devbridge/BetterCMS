using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaFolderViewModel : MediaViewModel
    {
        public virtual Guid ParentFolderId { get; set; }

        public MediaFolderViewModel()
        {
            ContentType = MediaContentType.Folder;
        }
    }
}