using System;
using System.IO;
using System.Threading.Tasks;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaFileService
    {
        void RemoveFile(Guid fileId, int version, bool doNotCheckVersion = false);

        MediaFile UploadFile(MediaType type, Guid rootFolderId, string fileName, long fileLength, Stream fileStream,
            bool isTemporary = true, string title = "", string description = "");

        MediaFile UploadFileWithStream(MediaType type, Guid rootFolderId, string fileName, long fileLength, Stream fileStream,
            bool WaitForUploadResult = false, string title = "", string description = "", MediaFile reuploadMediaFile = null);

        string CreateRandomFolderName();

        Uri GetFileUri(MediaType type, string folderName, string fileName);

        string GetPublicFileUrl(MediaType type, string folderName, string fileName);

        Task UploadMediaFileToStorageAsync<TMedia>(Stream sourceStream, Uri fileUri, Guid mediaId, Action<TMedia> updateMediaAfterUpload, Action<TMedia> updateMediaAfterFail, bool ignoreAccessControl) where TMedia : MediaFile;
        
        void UploadMediaFileToStorageSync<TMedia>(Stream sourceStream, Uri fileUri, TMedia media, Action<TMedia> updateMediaAfterUpload, Action<TMedia> updateMediaAfterFail, bool ignoreAccessControl) where TMedia : MediaFile;

        string GetDownloadFileUrl(MediaType type, Guid id, string fileUrl);

        void SaveMediaFile(MediaFile file);
    }
}