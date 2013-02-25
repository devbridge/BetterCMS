using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.MediaManager.ViewModels.Upload
{
    [Serializable]
    public class MultiFileUploadViewModel
    {
        public Guid RootFolderId { get; set; }

        public MediaType RootFolderType { get; set; }

        public Guid? SelectedFolderId { get; set; }

        public IList<Tuple<Guid, string>> Folders { get; set; }

        public IList<Guid> UploadedFiles { get; set; }
    }
}