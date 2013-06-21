using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Command.Upload.GetMultiFileUpload
{
    public class GetMultiFileUploadRequest
    {
        public Guid FolderId { get; set; }

        public MediaType Type { get; set; }

        public Guid ReuploadMediaId { get; set; }
    }
}