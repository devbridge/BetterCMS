using System;
using System.Configuration;
using System.IO;
using System.Net;
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
        protected const string TestBigImageFileName = "logo.big.png";
        protected const string TestBigImagePath = "BetterCms.Test.Module.Contents.Images.logo.big.png";
        protected const string TestImageCopyFileName = "logo.copy.png";
        protected const string TestImageMoveFileName = "logo.move.png";

        /// <summary>
        /// Mocks the configuration.
        /// </summary>
        /// <returns>Mocked CMS configuration</returns>
        protected ICmsConfiguration MockConfiguration(bool accessControlEnabled = false)
        {
            var serviceSection = (CmsTestConfigurationSection)ConfigurationManager.GetSection(CmsTestConfigurationSection.StorageSectionName);

            ICmsStorageConfiguration storageConfiguration = GetStorageConfiguration(serviceSection);
            if (storageConfiguration == null)
            {
                Assert.Ignore("Set up Config/storage.config values or Environment values for storage tests.");
            }

            Mock<ICmsConfiguration> cmsConfigurationMock = new Mock<ICmsConfiguration>();
            cmsConfigurationMock.Setup(f => f.Storage).Returns(storageConfiguration);

            var securityConfiguration = new Mock<ICmsSecurityConfiguration>();
            securityConfiguration.Setup(f => f.AccessControlEnabled).Returns(accessControlEnabled);

            cmsConfigurationMock.Setup(f => f.Security).Returns(securityConfiguration.Object);

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
        /// <param name="configuration">The configuration.</param>
        /// <param name="useSmallImage">if set to <c>true</c> [use small image].</param>
        /// <returns></returns>
        protected UploadRequest CreateUploadRequest(ICmsConfiguration configuration, bool useSmallImage = true)
        {
            string url = Path.Combine(configuration.Storage.ContentRoot, useSmallImage ? TestImageFileName : TestBigImageFileName);
            var fileUri = new Uri(url);
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(useSmallImage ? TestImagePath : TestBigImagePath);

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
        /// <param name="useSmallImage">if set to <c>true</c> [use small image].</param>
        protected void ShouldUploadObject(ICmsConfiguration configuration, IStorageService storageService, bool useSmallImage = true)
        {
            // Upload
            var request = CreateUploadRequest(configuration, useSmallImage);
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

        protected void ShouldDownloadUrlUnsecured(ICmsConfiguration configuration, IStorageService storageService)
        {
            // Upload
            var request = CreateUploadRequest(configuration);
            var uploadedSize = request.InputStream.Length;
            storageService.UploadObject(request);
            request.InputStream.Dispose();

            var downloadRequest = WebRequest.Create(request.Uri.AbsoluteUri);
            var response = downloadRequest.GetResponse();
            Assert.NotNull(response);
            Assert.AreEqual(response.ContentLength, uploadedSize);

            // Remove
            storageService.RemoveObject(request.Uri);
        }

        protected void ShouldNotDownloadUrlSecured(ICmsConfiguration configuration, IStorageService storageService)
        {
            // Upload
            var request = CreateUploadRequest(configuration);
            storageService.UploadObject(request);
            request.InputStream.Dispose();

            var downloadRequest = WebRequest.Create(request.Uri.AbsoluteUri);
            var failed = false;
            try
            {
                downloadRequest.GetResponse();
            }
            catch (WebException)
            {
                failed = true;
            }
            Assert.IsTrue(failed);

            // Remove
            storageService.RemoveObject(request.Uri);
        }

        protected void ShouldDownloadUrl(ICmsConfiguration configuration, IStorageService storageService)
        {
            // Upload
            var request = CreateUploadRequest(configuration);
            var uploadedSize = request.InputStream.Length;
            storageService.UploadObject(request);
            request.InputStream.Dispose();

            var url = storageService.GetSecuredUrl(request.Uri);
            Assert.AreNotEqual(url, request.Uri.AbsoluteUri);

            var downloadRequest = WebRequest.Create(url);
            var response = downloadRequest.GetResponse();
            Assert.NotNull(response);
            Assert.AreEqual(response.ContentLength, uploadedSize);

            // Remove
            storageService.RemoveObject(request.Uri);
        }
    }
}
