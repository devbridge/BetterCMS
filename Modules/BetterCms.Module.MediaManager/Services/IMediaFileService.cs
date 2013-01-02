using System;
using System.IO;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaFileService
    {
        void RemoveFile(Guid fileId, int version);
    }
}