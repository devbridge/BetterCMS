using System;
using System.IO;
using System.Net;

using BetterCms.Core.Services.Storage;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using StorageException = BetterCms.Core.Exceptions.Service.StorageException;

namespace BetterCms.Module.WindowsAzureStorage
{
    public class WindowsAzureStorageService : IStorageService
    {
        private readonly CloudStorageAccount cloudStorageAccount;

        private readonly string containerName;

        private readonly bool accessControlEnabled;
        
        private readonly TimeSpan tokenExpiryTime;

        // Allow resource to be cached by any cache for 7 days:
        private const string CacheControl = "public, max-age=604800";

        public WindowsAzureStorageService(ICmsConfiguration config)
        {
            try
            {
                var serviceSection = config.Storage;
                string accountName = serviceSection.GetValue("AzureAccountName");
                string secretKey = serviceSection.GetValue("AzureSecondaryKey");
                bool useHttps = bool.Parse(serviceSection.GetValue("AzureUseHttps"));
                
                tokenExpiryTime = TimeSpan.Parse(serviceSection.GetValue("AzureTokenExpiryTime"));
                accessControlEnabled = config.AccessControlEnabled;
                containerName = serviceSection.GetValue("AzureContainerName");

                cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(accountName, secretKey), useHttps);
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to initialize storage service {0}.", GetType()), e);
            }
        }

        public bool ObjectExists(Uri uri)
        {
            CheckUri(uri);

            try
            {
                var client = cloudStorageAccount.CreateCloudBlobClient();
                client.ParallelOperationThreadCount = 1;

                try
                {
                    var blob = client.GetBlobReferenceFromServer(uri);
                    return blob.Exists();
                }
                catch (Microsoft.WindowsAzure.Storage.StorageException ex)
                {
                    if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.NotFound)
                    {
                        return false;
                    }

                    // Status not found - throw the exception.
                    throw;
                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to check if object exists {0}.", uri), e);
            }
        }

        public void UploadObject(UploadRequest request)
        {
            CheckUri(request.Uri);

            try
            {
                var client = cloudStorageAccount.CreateCloudBlobClient();
                client.ParallelOperationThreadCount = 1;

                var container = client.GetContainerReference(containerName);
                if (request.CreateDirectory)
                {
                    container.CreateIfNotExists();
                }

                var permissions = new BlobContainerPermissions();
                if (accessControlEnabled)
                {
                    permissions.PublicAccess = BlobContainerPublicAccessType.Off;
                }
                else
                {
                    permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                }
                container.SetPermissions(permissions);

                var blob = container.GetBlockBlobReference(request.Uri.AbsoluteUri);

                blob.Properties.ContentType = MimeTypeUtility.DetermineContentType(request.Uri);
                blob.Properties.CacheControl = CacheControl;

                if (request.InputStream.Position != 0)
                {
                    request.InputStream.Position = 0;
                }

                blob.UploadFromStream(request.InputStream);
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to upload object with request {0}.", request), e);
            }
        }

        public DownloadResponse DownloadObject(Uri uri)
        {
            CheckUri(uri);

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                var response = request.GetResponse();
                var downloadResponse = new DownloadResponse();
                downloadResponse.Uri = uri;

                using (var responseStream = response.GetResponseStream())
                {
                    downloadResponse.ResponseStream = new MemoryStream();
                    if (responseStream != null)
                    {
                        responseStream.CopyTo(downloadResponse.ResponseStream);
                    }
                }

                return downloadResponse;
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to download object from {0}.", uri), e);
            }
        }

        public void CopyObject(Uri sourceUri, Uri destinationUri)
        {
            CheckUri(sourceUri);
            CheckUri(destinationUri);

            try
            {
                var client = cloudStorageAccount.CreateCloudBlobClient();
                var container = client.GetContainerReference(containerName);
                var destinationBlob = container.GetBlockBlobReference(destinationUri.AbsoluteUri);

                destinationBlob.StartCopyFromBlob(sourceUri);
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to copy object. SourceUrl: {0}, DestinationUrl: {1}", sourceUri, destinationUri), e);
            }
        }

        public void RemoveObject(Uri uri)
        {
            CheckUri(uri);
            try
            {
                var client = cloudStorageAccount.CreateCloudBlobClient();
                var container = client.GetContainerReference(containerName);
                var blob = container.GetBlockBlobReference(uri.AbsoluteUri);

                blob.DeleteIfExists();
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to delete object. Uri: {0}", uri), e);
            }
        }

        public void RemoveFolder(Uri uri)
        {
            CheckUri(uri);

            var client = cloudStorageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);
            var prefix = GetBlobDirectory(uri.AbsolutePath);

            var blobs = client.GetContainerReference(containerName);

            var blobsList = blobs.GetDirectoryReference(prefix).ListBlobs(true);
            try
            {
                foreach (var blob in blobsList)
                {
                    container.GetBlockBlobReference(blob.Uri.AbsoluteUri).DeleteIfExists();
                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to delete folder. Uri: {0}", uri), e);
            }
        }

        public string GetSecuredUrl(Uri uri)
        {
            CheckUri(uri);

            try
            {
                var client = cloudStorageAccount.CreateCloudBlobClient();
                client.ParallelOperationThreadCount = 1;

                var blob = client.GetBlobReferenceFromServer(uri);

                var sharedAccessPolicy = new SharedAccessBlobPolicy();
                sharedAccessPolicy.Permissions = SharedAccessBlobPermissions.Read;
                sharedAccessPolicy.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1);
                sharedAccessPolicy.SharedAccessExpiryTime = DateTime.UtcNow.Add(tokenExpiryTime);
                
                var sas = blob.GetSharedAccessSignature(sharedAccessPolicy);
                return string.Concat(uri, sas);
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to get shared access signature for. Uri: {0}.", uri), e);
            }
        }

        private string GetBlobDirectory(string path)
        {
            var index = path.LastIndexOf('/');
            var result = path.Remove(index);
            result = result.TrimStart('/');
            index = result.IndexOf('/');
            result = result.Substring(index + 1);

            return result;
        }

        private void CheckUri(Uri uri)
        {
            if (!Uri.CheckSchemeName(uri.Scheme) || !(uri.Scheme.Equals(Uri.UriSchemeHttp) || uri.Scheme.Equals(Uri.UriSchemeHttps)))
            {
                throw new StorageException(string.Format("An Uri scheme {0} is invalid. Uri {1} can't be processed with a {2} storage service.", uri.Scheme, uri, GetType().Name));
            }
        }
    }
}