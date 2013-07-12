using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.Viddler.Command.Videos.SaveVideos
{
    public class SaveVideosResponse
    {
        public List<MediaFileViewModel> Medias { get; set; }

        public Guid SelectedFolderId { get; set; }

        public SaveVideosResponse()
        {
            Medias = new List<MediaFileViewModel>();
        }

        public bool FolderIsDeleted { get; set; }

    }
}