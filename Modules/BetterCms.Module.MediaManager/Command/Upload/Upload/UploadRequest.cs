using System;
using System.IO;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.MediaManager.Command.Upload
{
    public class UploadFileRequest
    {
        public Guid RootFolderId { get; set; }

        public MediaType Type { get; set; }

        public string FileName { get; set; }
            
        public long FileLength { get; set; }

        public Stream FileStream { get; set; }
    }
}