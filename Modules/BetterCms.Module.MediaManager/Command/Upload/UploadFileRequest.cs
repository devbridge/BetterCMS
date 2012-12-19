using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

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
    }
}