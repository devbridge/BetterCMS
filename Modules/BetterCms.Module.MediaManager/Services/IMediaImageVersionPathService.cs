using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaImageVersionPathService
    {
        void SetPathForArchive(MediaImage archivedImage, string folder, string filename);

        void SetPathForNewOriginal(MediaImage newOriginalImage, string folderName, string fileName, Uri archivedImageOriginalUri = null, string archivedImagePublicOriginalUrl = "");
    }
}
