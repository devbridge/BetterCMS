using System;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.MediaManager.Command.Upload.GetMultiFileUpload
{
    public class GetMultiFileUploadRequest
    {
        public Guid FolderId { get; set; }

        public MediaType Type { get; set; }
    }
}