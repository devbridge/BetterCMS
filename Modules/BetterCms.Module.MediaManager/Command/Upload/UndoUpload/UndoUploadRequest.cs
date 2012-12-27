using System;

namespace BetterCms.Module.MediaManager.Command.Upload.UndoUpload
{
    public class UndoUploadRequest
    {
        public Guid FileId { get; set; }

        public int Version { get; set; }
    }
}