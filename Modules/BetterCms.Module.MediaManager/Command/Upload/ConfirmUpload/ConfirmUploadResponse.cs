using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.MediaManager.Command.Upload.ConfirmUpload
{    
    public class ConfirmUploadResponse
    {
        public List<MediaFileViewModel> Medias { get; set; }

        public Guid SelectedFolderId { get; set; }

        public ConfirmUploadResponse()
        {
            Medias = new List<MediaFileViewModel>();
        }

        public bool FolderIsDeleted { get; set; }

        public Guid ReuploadMediaId { get; set; }
    }
}
