using System;

namespace BetterCms.Core.Services.Storage
{
    public interface IStorageService
    {        
        bool ObjectExists(Uri uri);

        void UploadObject(UploadRequest request);

        DownloadResponse DownloadObject(Uri uri);

        void CopyObject(Uri sourceUri, Uri destinationUri);
    }
}
