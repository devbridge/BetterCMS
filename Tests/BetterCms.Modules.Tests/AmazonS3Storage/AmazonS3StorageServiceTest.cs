using System;
using System.IO;
using System.Reflection;

using BetterCms.Configuration;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.AmazonS3Storage;
using BetterCms.Test.Module.Configuration;

using NUnit.Framework;

namespace BetterCms.Test.Module.AmazonS3Storage
{
    [TestFixture]
    public class AmazonS3torageServiceTest : StorageTestBase
    {
        private const string AmazonAccessKey = "AmazonAccessKey";
        private const string AmazonSecretKey = "AmazonSecretKey";
        private const string AmazonBucketName = "AmazonBucketName";

        [Test]
        public void Should_Upload_Object()
        {
            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            // Upload
            var request = CreateUploadRequest(configuration);
            amazonStorageService.UploadObject(request);
            request.InputStream.Dispose();

            // Exists
            var exists = amazonStorageService.ObjectExists(request.Uri);
            Assert.IsTrue(exists);

            // Remove
            amazonStorageService.RemoveObject(request.Uri);
        }

        [Test]
        public void Should_Download_Object()
        {
            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            // Upload
            var request = CreateUploadRequest(configuration);
            var uploadedSize = request.InputStream.Length;
            amazonStorageService.UploadObject(request);
            request.InputStream.Dispose();

            // Download
            var file = amazonStorageService.DownloadObject(request.Uri);
            Assert.IsNotNull(file);
            Assert.IsNotNull(file.ResponseStream);
            Assert.IsTrue(file.ResponseStream.Length > 0);
            Assert.AreEqual(file.ResponseStream.Length, uploadedSize);

            // Remove
            amazonStorageService.RemoveObject(request.Uri);
        }

        [Test]
        public void Should_Copy_Object()
        {
            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            // Upload
            var request = CreateUploadRequest(configuration);
            amazonStorageService.UploadObject(request);
            request.InputStream.Dispose();

            // Copy
            string copyUrl = Path.Combine(configuration.Storage.ContentRoot, TestImageCopyFileName);
            var copyUri = new Uri(copyUrl);
            amazonStorageService.CopyObject(request.Uri, copyUri);

            // Exists
            var exists = amazonStorageService.ObjectExists(copyUri);
            Assert.IsTrue(exists);

            // Remove
            amazonStorageService.RemoveObject(request.Uri);
            amazonStorageService.RemoveObject(copyUri);
        }

        /// <summary>
        /// Creates the upload request.
        /// </summary>
        /// <returns></returns>
        private UploadRequest CreateUploadRequest(ICmsConfiguration configuration)
        {
            string url = Path.Combine(configuration.Storage.ContentRoot, TestImageFileName);
            var fileUri = new Uri(url);
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(TestImagePath);

            var request = new UploadRequest
            {
                InputStream = stream,
                Uri = fileUri
            };

            return request;
        }

        /// <summary>
        /// Gets the storage configuration.
        /// </summary>
        /// <param name="serviceSection">The service section.</param>
        /// <returns>
        /// Storage Configuration
        /// </returns>
        protected override ICmsStorageConfiguration GetStorageConfiguration(CmsTestConfigurationSection serviceSection)
        {
            var accessKey = serviceSection.AmazonStorage.GetValue(AmazonAccessKey);
            var secretKey = serviceSection.AmazonStorage.GetValue(AmazonSecretKey);

            if (!string.IsNullOrWhiteSpace(accessKey) && !string.IsNullOrWhiteSpace(secretKey))
            {
                return serviceSection.AmazonStorage;
            }

            accessKey = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_ACCESS_KEY", EnvironmentVariableTarget.User);
            secretKey = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_SECRET_KEY", EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(accessKey) || !string.IsNullOrWhiteSpace(secretKey))
            {
                var bucketName = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_BUCKET_NAME", EnvironmentVariableTarget.User);

                var configuration = new CmsStorageConfigurationElement
                    {
                        ContentRoot = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_CONTENT_ROOT", EnvironmentVariableTarget.User),
                        PublicContentUrlRoot = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_PUBLIC_CONTENT_ROOT", EnvironmentVariableTarget.User),
                        ServiceType = StorageServiceType.Auto
                    };

                configuration.Add(new KeyValueElement {Key = AmazonAccessKey, Value = accessKey});
                configuration.Add(new KeyValueElement {Key = AmazonSecretKey, Value = secretKey});
                configuration.Add(new KeyValueElement {Key = AmazonBucketName, Value = bucketName});

                return configuration;
            }

            return null;
        }
    }
}
