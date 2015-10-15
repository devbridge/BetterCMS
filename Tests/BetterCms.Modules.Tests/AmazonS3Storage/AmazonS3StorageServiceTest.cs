using System;

using BetterCms.Configuration;
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
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldUploadObject(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Download_Object()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldDownloadObject(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Copy_Object()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldCopyObject(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Download_Url_Unsecured()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldDownloadUrlUnsecured(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Not_Download_Url_Secured()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration(true);
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldNotDownloadUrlSecured(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Download_Url_Secured()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration(true);
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldDownloadUrl(configuration, amazonStorageService);
        }
        
        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Should_Fail_Timeout()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration(true);
            var amazonStorageService = new AmazonS3StorageService(configuration);
            amazonStorageService.Timeout = 1;

            ShouldUploadObject(configuration, amazonStorageService, false);
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

            accessKey = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_ACCESS_KEY", EnvironmentVariableTarget.Machine);
            secretKey = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_SECRET_KEY", EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrWhiteSpace(accessKey) || !string.IsNullOrWhiteSpace(secretKey))
            {
                var bucketName = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_BUCKET_NAME", EnvironmentVariableTarget.Machine);

                var configuration = new CmsStorageConfigurationElement
                    {
                        ContentRoot = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_CONTENT_ROOT", EnvironmentVariableTarget.Machine),
                        PublicContentUrlRoot = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_PUBLIC_CONTENT_ROOT", EnvironmentVariableTarget.Machine),
                        ServiceType = StorageServiceType.Auto,
                        ProcessTimeout = serviceSection.AmazonStorage.ProcessTimeout
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
