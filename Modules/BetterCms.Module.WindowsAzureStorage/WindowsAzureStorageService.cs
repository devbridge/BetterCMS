using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using BetterCms.Core.Services.Storage;
using BetterCms.Module.WindowsAzureStorage.Content.Resources;

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
        
        private readonly string securedContainerName;

        private readonly bool accessControlEnabledGlobally;
        
        private readonly TimeSpan tokenExpiryTime;

        private TimeSpan timeout;

        private string securedContainerIssue;

        // Allow resource to be cached by any cache for 7 days:
        private const string CacheControl = "public, max-age=604800";

        public TimeSpan Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public WindowsAzureStorageService(ICmsConfiguration config)
        {
            try
            {
                var serviceSection = config.Storage;
                string accountName = serviceSection.GetValue("AzureAccountName");
                string secretKey = serviceSection.GetValue("AzureSecondaryKey");
                bool useHttps = bool.Parse(serviceSection.GetValue("AzureUseHttps"));

                if (!TimeSpan.TryParse(serviceSection.GetValue("AzureTokenExpiryTime"), out tokenExpiryTime))
                {
                    tokenExpiryTime = TimeSpan.FromMinutes(10);
                }

                timeout = serviceSection.ProcessTimeout;
                
                accessControlEnabledGlobally = config.Security.AccessControlEnabled;
                containerName = serviceSection.GetValue("AzureContainerName");
                securedContainerName = serviceSection.GetValue("AzureSecuredContainerName");
                if (string.IsNullOrWhiteSpace(securedContainerName))
                {
                    securedContainerName = containerName;
                }

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
                    var blob = client.GetBlobReferenceFromServer(uri, options: new BlobRequestOptions { MaximumExecutionTime = timeout, ServerTimeout = timeout });
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

                var securityEnabled = accessControlEnabledGlobally && !request.IgnoreAccessControl;
                var currentContainerName = securityEnabled ? securedContainerName : containerName;

                // Create container with specified security level 
                var container = client.GetContainerReference(currentContainerName);
                if (request.CreateDirectory)
                {
                    if (container.CreateIfNotExists())
                    {
                        var permissions = new BlobContainerPermissions();
                        if (securityEnabled)
                        {
                            permissions.PublicAccess = BlobContainerPublicAccessType.Off;
                        }
                        else
                        {
                            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                        }
                        container.SetPermissions(permissions);
                    }
                }

                var blob = container.GetBlockBlobReference(request.Uri.AbsoluteUri);

                blob.Properties.ContentType = MimeTypeUtility.DetermineContentType(request.Uri);
                blob.Properties.CacheControl = CacheControl;

                if (request.InputStream.Position != 0)
                {
                    request.InputStream.Position = 0;
                }

                blob.UploadFromStream(request.InputStream, options: new BlobRequestOptions { MaximumExecutionTime = timeout, ServerTimeout = timeout });
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
                var timeoutMs = timeout.TotalMilliseconds <= Int32.MaxValue ? Convert.ToInt32(timeout.TotalMilliseconds) : Int32.MaxValue;

                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Timeout = timeoutMs;
                request.ReadWriteTimeout = timeoutMs;

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

                destinationBlob.StartCopyFromBlob(sourceUri, options: new BlobRequestOptions { MaximumExecutionTime = timeout, ServerTimeout = timeout });
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

                blob.DeleteIfExists(options: new BlobRequestOptions { MaximumExecutionTime = timeout, ServerTimeout = timeout });
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

            var blobsList = blobs.GetDirectoryReference(prefix).ListBlobs(true, options: new BlobRequestOptions { MaximumExecutionTime = timeout, ServerTimeout = timeout });
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

        public bool SecuredUrlsEnabled
        {
            get { return true; }
        }

        public string SecuredContainerIssueWarning
        {
            get
            {
                if (securedContainerIssue == null)
                {
                    securedContainerIssue = string.Empty;
                    if (accessControlEnabledGlobally)
                    {
                        try
                        {
                            var client = cloudStorageAccount.CreateCloudBlobClient();
                            var container = client.GetContainerReference(securedContainerName);
                            var permission = container.GetPermissions();
                            if (permission.PublicAccess == BlobContainerPublicAccessType.Container)
                            {
                                securedContainerIssue = AzureGlobalization.TokenBasedSecurity_HasSecuredContainerIssue_Message;
                            }
                        }
                        catch (Exception e)
                        {
                            throw new StorageException(string.Format("Failed to check container permissions."), e);
                        }
                    }
                }

                return !string.IsNullOrWhiteSpace(securedContainerIssue) ? securedContainerIssue : null;
            }
        }

        public string GetSecuredUrl(Uri uri)
        {
            CheckUri(uri);

            try
            {
                var client = cloudStorageAccount.CreateCloudBlobClient();
                client.ParallelOperationThreadCount = 1;

                var blob = client.GetBlobReferenceFromServer(uri, options: new BlobRequestOptions { MaximumExecutionTime = timeout, ServerTimeout = timeout });

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