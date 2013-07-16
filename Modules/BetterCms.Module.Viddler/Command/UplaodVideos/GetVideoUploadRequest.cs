using System;

namespace BetterCms.Module.Viddler.Command.UplaodVideos
{
    public class GetVideoUploadRequest
    {
        public Guid FolderId { get; set; }

        public Guid ReuploadMediaId { get; set; }
    }
}