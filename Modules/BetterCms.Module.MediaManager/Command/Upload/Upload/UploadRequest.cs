using System;
using System.IO;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Command.Upload
{
    public class UploadFileRequest
    {
        public Guid RootFolderId { get; set; }

        public MediaType Type { get; set; }

        public string FileName { get; set; }
            
        public long FileLength { get; set; }

        public Stream FileStream { get; set; }

        public Guid ReuploadMediaId { get; set; }

        public bool ShouldOverride { get; set; }
    }
}