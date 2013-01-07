using System.Collections.Generic;

using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.MediaManager.Command.Upload.ConfirmUpload
{    
    public class ConfirmUploadResponse
    {
        public List<MediaFileViewModel> Medias { get; set; }

        public ConfirmUploadResponse()
        {
            Medias = new List<MediaFileViewModel>();
        }
    }
}
