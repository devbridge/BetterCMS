using System;
using System.Collections.Generic;

namespace BetterCms.Module.Viddler.Command.Videos.SaveVideos
{
    public class SaveVideosRequest
    {
        public Guid RootFolderId { get; set; }

        public Guid ReuploadMediaId { get; set; }

        public Guid? SelectedFolderId { get; set; }

        public IList<Guid> UploadedFiles { get; set; }
    }
}