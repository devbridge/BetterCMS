using System;
using System.Drawing;
using System.IO;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaImageService
    {
        MediaImage UploadImage(Guid rootFolderId, string fileName, long fileLength, Stream file, Guid reuploadMediaId, MediaImage filledInImage = null);

        void RemoveImageWithFiles(Guid mediaImageId, int version, bool doNotCheckVersion = false, bool originalWasNotUploaded = false);

        void UpdateThumbnail(MediaImage mediaImage, Size size);

        MediaImage MakeAsOriginal(MediaImage image, MediaImage originalImage, MediaImage archivedImage);

        void SaveEditedImage(MediaImage image, MediaImage archivedImage, MemoryStream croppedImageFileStream);

        MediaImage MoveToHistory(MediaImage originalImage);
    }
}