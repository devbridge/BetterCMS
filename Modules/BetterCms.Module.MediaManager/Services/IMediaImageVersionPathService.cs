using System;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Enum;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaImageVersionPathService
    {
        void SetPathForArchive(MediaImage archivedImage, string folder, string filename);

        void SetPathForNewOriginal(MediaImage newOriginalImage, string folderName, string fileName, ImageType imageType, Uri archivedImageOriginalUri = null, string archivedImagePublicOriginalUrl = "");
    }
}
