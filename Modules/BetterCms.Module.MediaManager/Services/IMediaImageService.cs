using System;
using System.IO;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaImageService
    {
        MediaImage UploadImage(Guid rootFolderId, string fileName, long fileLength, Stream file);

        void CropImage(Guid mediaImageId, int version, int x1, int y1, int x2, int y2);

        void ResizeImage(Guid mediaImageId, int version, int width, int height);

        void RemoveImageWithFiles(Guid mediaImageId, int version);
    }
}