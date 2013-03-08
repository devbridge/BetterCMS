using System;
using System.IO;
using System.Threading.Tasks;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaFileService
    {
        void RemoveFile(Guid fileId, int version);

        string GetFileSizeText(long sizeInBytes);

        MediaFile UploadFile(MediaType type, Guid rootFolderId, string fileName, long fileLength, Stream fileStream);

        string CreateRandomFolderName();

        Uri GetFileUri(MediaType type, string folderName, string fileName);

        string GetPublicFileUrl(MediaType type, string folderName, string fileName);

        /// <summary>
        /// Creates a task to upload a file to the storage.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="fileUri">The file URI.</param>
        /// <param name="mediaId">The media id.</param>
        /// <param name="updateMediaAfterUpload">An action to update a specific field for the media after image upload.</param>
        /// <returns>
        /// Upload file task.
        /// </returns>
        Task UploadMediaFileToStorage<TMedia>(Stream sourceStream, Uri fileUri, Guid mediaId, Action<TMedia> updateMediaAfterUpload) where TMedia : MediaFile;
    }
}