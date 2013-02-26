using System;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Command.Upload.UndoUpload
{
    public class UndoUploadRequest
    {
        public Guid FileId { get; set; }

        public int Version { get; set; }

        public MediaType Type { get; set; }
    }
}