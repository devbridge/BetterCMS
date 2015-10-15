using System;
using System.IO;
using System.Net;
using System.Web;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Services.Storage;

namespace BetterCms.Module.AmazonS3Storage
{
    public class AmazonS3StorageService : IStorageService
    {
        private const int DefaultTimeout = 300000;

        private readonly string accessKey;
        private readonly string secretKey;
        private readonly string bucketName;
        private int timeout;

        private readonly bool accessControlEnabledGlobally;

        private readonly TimeSpan tokenExpiryTime;

        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public AmazonS3StorageService(ICmsConfiguration config)
        {
            try
            {                
                var serviceSection = config.Storage;

                accessKey = serviceSection.GetValue("AmazonAccessKey");
                secretKey = serviceSection.GetValue("AmazonSecretKey");
                bucketName = serviceSection.GetValue("AmazonBucketName");

                try
                {
                    if (serviceSection.ProcessTimeout.TotalMilliseconds > Int32.MaxValue)
                    {
                        timeout = Int32.MaxValue;
                    }
                    else
                    {
                        timeout = Convert.ToInt32(serviceSection.ProcessTimeout.TotalMilliseconds);
                        if (timeout <= 0)
                        {
                            timeout = DefaultTimeout;
                        }
                    }
                }
                catch (OverflowException)
                {
                    timeout = DefaultTimeout;
                }

                if (!TimeSpan.TryParse(serviceSection.GetValue("AmazonTokenExpiryTime"), out tokenExpiryTime))
                {
                    tokenExpiryTime = TimeSpan.FromMinutes(10);
                }
                accessControlEnabledGlobally = config.Security.AccessControlEnabled;
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
                using (var client = CreateAmazonS3Client())
                {
                    try
                    {
                        var absolutePath = HttpUtility.UrlDecode(uri.AbsolutePath);
                        var key = absolutePath.TrimStart('/');
                        var request = new GetObjectMetadataRequest();

                        request.WithBucketName(bucketName)
                            .WithKey(key)
                            .WithTimeout(timeout)
                            .WithReadWriteTimeout(timeout);

                        using (client.GetObjectMetadata(request))
                        {
                            return true;
                        }
                    }
                    catch (AmazonS3Exception ex)
                    {
                        if (ex.StatusCode == HttpStatusCode.NotFound)
                        {
                            return false;
                        }

                        // Status not found - throw the exception.
                        throw;
                    }
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
                var putRequest = new PutObjectRequest();
                using (var client = CreateAmazonS3Client())
                {
                    var absolutePath = HttpUtility.UrlDecode(request.Uri.AbsolutePath);
                    var key = absolutePath.TrimStart(Convert.ToChar("/"));

                    putRequest.WithBucketName(bucketName)
                        .WithKey(key)
                        .WithInputStream(request.InputStream)
                        .WithTimeout(timeout)
                        .WithReadWriteTimeout(timeout);

                    if (accessControlEnabledGlobally && !request.IgnoreAccessControl)
                    {
                        putRequest.WithCannedACL(S3CannedACL.Private);
                    }
                    else
                    {
                        putRequest.WithCannedACL(S3CannedACL.PublicRead);
                    }

                    if (request.Headers != null && request.Headers.Count > 0)
                    {
                        putRequest.AddHeaders(request.Headers);
                    }

                    if (request.MetaData != null && request.MetaData.Count > 0)
                    {
                        putRequest.WithMetaData(request.MetaData);
                    }

                    putRequest.ContentType = MimeTypeUtility.DetermineContentType(request.Uri);

                    using (client.PutObject(putRequest))
                    {
                    }
                }
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
                request.Timeout = timeout;
                request.ReadWriteTimeout = timeout;

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
                var sourceKey = HttpUtility.UrlDecode(sourceUri.AbsolutePath).TrimStart('/');
                var destinationKey = HttpUtility.UrlDecode(destinationUri.AbsolutePath).TrimStart('/');

                using (var client = CreateAmazonS3Client())
                {
                    var request = new CopyObjectRequest()
                        .WithSourceBucket(bucketName)
                        .WithDestinationBucket(bucketName)
                        .WithCannedACL(S3CannedACL.PublicRead)
                        .WithSourceKey(sourceKey)
                        .WithDestinationKey(destinationKey)
                        .WithDirective(S3MetadataDirective.COPY)
                        .WithTimeout(timeout);

                    client.CopyObject(request);

                }
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
                var sourceKey = HttpUtility.UrlDecode(uri.AbsolutePath).TrimStart('/');                

                using (var client = CreateAmazonS3Client())
                {
                    var request = new DeleteObjectRequest { Timeout = timeout, ReadWriteTimeout = timeout}
                        .WithKey(sourceKey)
                        .WithBucketName(bucketName);

                    client.DeleteObject(request);                    
                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to delete object. Uri: {0}", uri), e);
            }
        }

        public void RemoveFolder(Uri uri)
        {
            CheckUri(uri);

            try
            {
                var sourceKey = HttpUtility.UrlDecode(uri.AbsolutePath).TrimStart('/');

                using (var client = CreateAmazonS3Client())
                {
                    var request = new DeleteObjectRequest { Timeout = timeout, ReadWriteTimeout = timeout}
                        .WithKey(sourceKey)
                        .WithBucketName(bucketName);

                    client.DeleteObject(request);
                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to delete object. Uri: {0}", uri), e);
            }
        }

        private AmazonS3 CreateAmazonS3Client()
        {
            return AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey);
        }

        private void CheckUri(Uri uri)
        {
            if (!Uri.CheckSchemeName(uri.Scheme) || !(uri.Scheme.Equals(Uri.UriSchemeHttp) || uri.Scheme.Equals(Uri.UriSchemeHttps)))
            {
                throw new StorageException(string.Format("An Uri scheme {0} is invalid. Uri {1} can't be processed with a {2} storage service.", uri.Scheme, uri, GetType().Name));
            }
        }

        public bool SecuredUrlsEnabled
        {
            get { return true; }
        }

        public string SecuredContainerIssueWarning
        {
            get { return null; }
        }

        public string GetSecuredUrl(Uri uri)
        {
            CheckUri(uri);

            try
            {
                using (var client = CreateAmazonS3Client())
                {
                    var absolutePath = HttpUtility.UrlDecode(uri.AbsolutePath);
                    var key = absolutePath.TrimStart('/');
                    var request = new GetPreSignedUrlRequest();

                    request.WithBucketName(bucketName)
                        .WithKey(key)
                        .WithExpires(DateTime.UtcNow.Add(tokenExpiryTime))
                        .WithTimeout(timeout)
                        .WithReadWriteTimeout(timeout);

                    var url = client.GetPreSignedURL(request);
                    return url;
                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to get signed URL for file. Uri: {0}.", uri), e);
            }
        }
    }
}
