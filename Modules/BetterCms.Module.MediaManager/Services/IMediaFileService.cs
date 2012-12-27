using System.IO;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaFileService
    {
        MediaFile UploadTemporaryFile(MediaType type, string fileName, Stream inputStream);

        void RemoveFile(MediaFile file);
    }
}