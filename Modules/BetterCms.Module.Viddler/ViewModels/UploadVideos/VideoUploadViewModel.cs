using System;
using System.Collections.Generic;

namespace BetterCms.Module.Viddler.ViewModels.UploadVideos
{
    public class VideoUploadViewModel
    {
        public Guid RootFolderId { get; set; }

        public Guid ReuploadMediaId { get; set; }

        public Guid SelectedFolderId { get; set; }

        public List<Tuple<Guid, string>> Folders { get; set; }
    }
}