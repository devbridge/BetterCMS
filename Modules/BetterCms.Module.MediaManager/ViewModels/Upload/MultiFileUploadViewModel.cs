using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.MediaManager.ViewModels.Upload
{
    [Serializable]
    public class MultiFileUploadViewModel
    {
        public Guid RootFolderId { get; set; }

        public Guid? SelectedFolderId { get; set; }

        public IDictionary<Guid, string> Folders { get; set; }
    }
}