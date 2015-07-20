using System;

namespace BetterCms.Core.Services.Storage
{
    public interface IStorageService
    {        
        bool ObjectExists(Uri uri);

        void UploadObject(UploadRequest request);

        DownloadResponse DownloadObject(Uri uri);

        void CopyObject(Uri sourceUri, Uri destinationUri);

        void RemoveObject(Uri uri);

        void MoveObject(Uri sourceUri, Uri destinationUri, bool createDirectoriesIfNotExists = true);

        void RemoveFolder(Uri uri);

        string GetSecuredUrl(Uri uri);
        
        bool SecuredUrlsEnabled { get; }

        string SecuredContainerIssueWarning { get; }
    }
}
