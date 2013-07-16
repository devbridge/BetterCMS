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

        public string SessionId { get; set; }

        public string Token { get; set; }

        public string Endpoint { get; set; }

        public string CallbackUrl { get; set; }

        public IList<Guid> UploadedFiles { get; set; }
    }
}