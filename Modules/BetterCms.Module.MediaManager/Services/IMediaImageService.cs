using System;
using System.Drawing;
using System.IO;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaImageService
    {
        MediaImage UploadImage(Guid rootFolderId, string fileName, long fileLength, Stream file);

        void RemoveImageWithFiles(Guid mediaImageId, int version);

        void UpdateThumbnail(MediaImage mediaImage, Size size);
    }
}