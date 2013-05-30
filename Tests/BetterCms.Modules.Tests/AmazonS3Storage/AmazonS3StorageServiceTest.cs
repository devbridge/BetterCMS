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
            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldUploadObject(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Download_Object()
        {
            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldDownloadObject(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Copy_Object()
        {
            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldCopyObject(configuration, amazonStorageService);
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
