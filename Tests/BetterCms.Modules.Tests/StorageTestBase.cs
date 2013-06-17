using System;
using System.Configuration;
using System.IO;
using System.Reflection;

using BetterCms.Core.Services.Storage;
using BetterCms.Test.Module.Configuration;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module
{
    public abstract class StorageTestBase : TestBase
    {
        protected const string TestImagePath = "BetterCms.Test.Module.Contents.Images.logo.png";
        protected const string TestImageFileName = "logo.png";
        protected const string TestImageCopyFileName = "logo.copy.png";

        /// <summary>
        /// Mocks the configuration.
        /// </summary>
        /// <returns>Mocked CMS configuration</returns>
        protected ICmsConfiguration MockConfiguration()
        {
            var serviceSection = (CmsTestConfigurationSection)ConfigurationManager.GetSection(CmsTestConfigurationSection.StorageSectionName);

            ICmsStorageConfiguration storageConfiguration = GetStorageConfiguration(serviceSection);
            if (storageConfiguration == null)
            {
                Assert.Ignore("Set up Config/storage.config values or Environment values for storage tests.");
            }

            Mock<ICmsConfiguration> cmsConfigurationMock = new Mock<ICmsConfiguration>();
            cmsConfigurationMock.Setup(f => f.Storage).Returns(storageConfiguration);

            return cmsConfigurationMock.Object;
        }

        /// <summary>
        /// Gets the storage configuration.
        /// </summary>
        /// <param name="serviceSection">The service section.</param>
        /// <returns>Storage Configuration</returns>
        protected abstract ICmsStorageConfiguration GetStorageConfiguration(CmsTestConfigurationSection serviceSection);

        /// <summary>
        /// Creates the upload request.
        /// </summary>
        /// <returns></returns>
        protected UploadRequest CreateUploadRequest(ICmsConfiguration configuration)
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
        /// Should upload file to storage successfuly.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="storageService">The storage service.</param>
        protected void ShouldUploadObject(ICmsConfiguration configuration, IStorageService storageService)
        {
            // Upload
            var request = CreateUploadRequest(configuration);
            storageService.UploadObject(request);
            request.InputStream.Dispose();

            // Remove
            storageService.RemoveObject(request.Uri);
        }

        /// <summary>
        /// Should download file from storage successfully.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="storageService">The storage service.</param>
        protected void ShouldDownloadObject(ICmsConfiguration configuration, IStorageService storageService)
        {
            // Upload
            var request = CreateUploadRequest(configuration);
            var uploadedSize = request.InputStream.Length;
            storageService.UploadObject(request);
            request.InputStream.Dispose();

            // Download
            var file = storageService.DownloadObject(request.Uri);
            Assert.IsNotNull(file);
            Assert.IsNotNull(file.ResponseStream);
            Assert.IsTrue(file.ResponseStream.Length > 0);
            Assert.AreEqual(file.ResponseStream.Length, uploadedSize);

            // Remove
            storageService.RemoveObject(request.Uri);
        }

        /// <summary>
        /// Should copy storage file successfuly.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="storageService">The storage service.</param>
        protected void ShouldCopyObject(ICmsConfiguration configuration, IStorageService storageService)
        {
            // Upload
            var request = CreateUploadRequest(configuration);
            storageService.UploadObject(request);
            request.InputStream.Dispose();

            // Copy
            string copyUrl = Path.Combine(configuration.Storage.ContentRoot, TestImageCopyFileName);
            var copyUri = new Uri(copyUrl);
            storageService.CopyObject(request.Uri, copyUri);

            // Exists
            var exists = storageService.ObjectExists(copyUri);
            Assert.IsTrue(exists);

            // Remove
            storageService.RemoveObject(request.Uri);
            storageService.RemoveObject(copyUri);
        }
    }
}
