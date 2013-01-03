using System;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaFileService
    {
        void RemoveFile(Guid fileId, int version);

        string GetFileSizeText(long sizeInBytes);
    }
}