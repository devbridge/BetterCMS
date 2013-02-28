using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;

using BetterCms.Core.Services.Storage;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using StorageException = BetterCms.Core.Exceptions.Service.StorageException;

namespace BetterCms.Module.AmazonS3Storage
{
    public class WindowsAzureStorageService : IStorageService
    {
        private readonly CloudStorageAccount cloudStorageAccount;

        private readonly string containerName;

        public WindowsAzureStorageService(ICmsConfiguration config)
        {
            try
            {
                var serviceSection = config.Storage;
                string accountName = serviceSection.GetValue("AzureAccountName");
                string secretKey = serviceSection.GetValue("AzureSecondaryKey");                               
                bool useHttps = bool.Parse(serviceSection.GetValue("AzureUseHttps"));

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

                var blob = container.GetBlockBlobReference(request.Uri.AbsoluteUri);
                if (request.Headers != null && request.Headers.Count > 0)
                {
                    if (request.Headers["content-type"] != null)
                    {
                        blob.Properties.ContentType = request.MetaData["content-type"];
                    }
                }


                if (request.MetaData != null && request.MetaData.Count > 0)
                {                    
                    foreach (KeyValuePair<string, string> metadata in request.MetaData)
                    {
                        blob.Metadata.Add(metadata);
                    }                    
                }



/*
                blob.UploadFromStream(request.InputStream);
                putRequest.WithBucketName(containerName).WithKey(key).WithCannedACL(S3CannedACL.PublicRead).WithInputStream(request.InputStream);

                if (request.Headers != null && request.Headers.Count > 0)
                {
                    putRequest.AddHeaders(request.Headers);
                }

                if (request.MetaData != null && request.MetaData.Count > 0)
                {
                    putRequest.WithMetaData(request.MetaData);
                }*/
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

        /*    try
            {
                var sourceKey = sourceUri.AbsolutePath.TrimStart('/');
                var destinationKey = destinationUri.AbsolutePath.TrimStart('/');

                using (var client = CreateAmazonS3Client())
                {
                    var request = new CopyObjectRequest()
                        .WithSourceBucket(containerName)
                        .WithDestinationBucket(containerName)
                        .WithCannedACL(S3CannedACL.PublicRead)
                        .WithSourceKey(sourceKey)
                        .WithDestinationKey(destinationKey)
                        .WithDirective(S3MetadataDirective.COPY);

                    client.CopyObject(request);

                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to copy object. SourceUrl: {0}, DestinationUrl: {1}", sourceUri, destinationUri), e);
            }*/
        }

        public void RemoveObject(Uri uri)
        {
            CheckUri(uri);

        /*    try
            {
                var sourceKey = uri.AbsolutePath.TrimStart('/');

                using (var client = CreateAmazonS3Client())
                {
                    var request = new DeleteObjectRequest()
                        .WithKey(sourceKey)
                        .WithBucketName(containerName);

                    client.DeleteObject(request);
                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to delete object. Uri: {0}", uri), e);
            }*/
        }

        public void RemoveObjectBucket(Uri uri)
        {
            CheckUri(uri);

    /*        try
            {
                var sourceKey = uri.AbsolutePath.TrimStart('/');

                using (var client = CreateAmazonS3Client())
                {
                    var request = new DeleteObjectRequest()
                        .WithKey(sourceKey)
                        .WithBucketName(containerName);

                    client.DeleteObject(request);
                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to delete object. Uri: {0}", uri), e);
            }*/
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