using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class UploadViewModel
    {
        public Guid CurrentFolderId { get; set; }

        public IDictionary<Guid, string> Folders { get; set; }
    }
}