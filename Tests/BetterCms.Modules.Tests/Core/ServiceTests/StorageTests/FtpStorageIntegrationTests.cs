using System;

using BetterCms.Configuration;
using BetterCms.Core.Services.Storage;

using NUnit.Framework;

namespace BetterCms.Test.Module.Core.ServiceTests.StorageTests
{
    [TestFixture]
    public class FtpStorageIntegrationTests : StorageTestBase
    {
        private const string FtpUserName = "FtpUserName";
        private const string FtpPassword = "FtpPassword";
        private const string FtpUsePassiveMode = "UsePassiveMode";
        private const string FtpRootUrl = "FtpRoot";

        [Test]
        public void Should_Upload_Object()
        {
            var configuration = MockConfiguration();
            var ftpStorageService = new FtpStorageService(configuration);

            ShouldUploadObject(configuration, ftpStorageService);
        }

        [Test]
        public void Should_Download_Object()
        {
            var configuration = MockConfiguration();
            var ftpStorageService = new FtpStorageService(configuration);

            ShouldDownloadObject(configuration, ftpStorageService);
        }

        [Test]
        public void Should_Copy_Object()
        {
            var configuration = MockConfiguration();
            var ftpStorageService = new FtpStorageService(configuration);

            ShouldCopyObject(configuration, ftpStorageService);
        }

        /// <summary>
        /// Gets the storage configuration.
        /// </summary>
        /// <param name="serviceSection">The service section.</param>
        /// <returns>
        /// Storage Configuration
        /// </returns>
        protected override ICmsStorageConfiguration GetStorageConfiguration(Configuration.CmsTestConfigurationSection serviceSection)
        {
            var userName = serviceSection.FtpStorage.GetValue(FtpUserName);
            var password = serviceSection.FtpStorage.GetValue(FtpPassword);

            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                return serviceSection.FtpStorage;
            }

            userName = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_USER_NAME", EnvironmentVariableTarget.User);
            password = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_PASSWORD", EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(userName) || !string.IsNullOrWhiteSpace(password))
            {
                var usePassiveMode = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_USE_PASSIVE_MODE", EnvironmentVariableTarget.User);
                var ftpRoot = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_FTP_ROOT", EnvironmentVariableTarget.User);

                var configuration = new CmsStorageConfigurationElement
                {
                    ContentRoot = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_CONTENT_ROOT", EnvironmentVariableTarget.User),
                    PublicContentUrlRoot = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_CONTENT_ROOT_URL", EnvironmentVariableTarget.User),
                    ServiceType = StorageServiceType.Ftp
                };

                configuration.Add(new KeyValueElement { Key = FtpUserName, Value = userName });
                configuration.Add(new KeyValueElement { Key = FtpPassword, Value = password });
                configuration.Add(new KeyValueElement { Key = FtpUsePassiveMode, Value = usePassiveMode });
                configuration.Add(new KeyValueElement { Key = FtpRootUrl, Value = ftpRoot });

                return configuration;
            }

            return null;
        }
    }
}
