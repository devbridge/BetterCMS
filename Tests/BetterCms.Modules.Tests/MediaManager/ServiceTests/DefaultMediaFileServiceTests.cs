using System;

using BetterCms.Core.Services;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.ServiceTests
{
    public class DefaultMediaFileServiceTests :TestBase
    {
        protected const string PublicUrl1 = "http://PublicUrl1";
        protected const string SecuredUrl1 = "http://SecuredUrl1";

        [Test]
        public void Should_Return_Unsecured_Correct_File_PublicUrl()
        {
            var mediaFileService = GetMediaFileService(false);
            var url = mediaFileService.GetDownloadFileUrl(MediaType.File, Guid.Empty, PublicUrl1);

            Assert.AreEqual(url, PublicUrl1);
        }

        [Test]
        public void Should_Return_Unsecured_Correct_Image_PublicUrl()
        {
            var mediaFileService = GetMediaFileService(false);
            var url = mediaFileService.GetDownloadFileUrl(MediaType.Image, Guid.Empty, PublicUrl1);

            Assert.AreEqual(url, PublicUrl1);
        }

        [Test]
        public void Should_Return_Correct_Secured_File_PublicUrl()
        {
            var mediaFileService = GetMediaFileService(true);
            var url = mediaFileService.GetDownloadFileUrl(MediaType.File, Guid.Empty, PublicUrl1);

            Assert.AreEqual(url, SecuredUrl1);
        }

        [Test]
        public void Should_Return_Correct_Secured_Image_PublicUrl()
        {
            var mediaFileService = GetMediaFileService(true);
            var url = mediaFileService.GetDownloadFileUrl(MediaType.Image, Guid.Empty, PublicUrl1);

            Assert.AreEqual(url, PublicUrl1);
        }

        private IMediaFileService GetMediaFileService(bool secured)
        {
            var storageService = new Mock<IStorageService>();
            storageService.Setup(x => x.SecuredUrlsEnabled).Returns(secured);

            var securityConfig = new Mock<ICmsSecurityConfiguration>();
            securityConfig.Setup(f => f.AccessControlEnabled).Returns(secured);

            var cmsConfiguration = new Mock<ICmsConfiguration>();
            cmsConfiguration.Setup(x => x.Security).Returns(securityConfig.Object);

            var urlResolver = new Mock<IMediaFileUrlResolver>();
            urlResolver.Setup(x => x.GetMediaFileFullUrl(It.IsAny<Guid>(), It.IsAny<string>())).Returns(SecuredUrl1);

            var securityService = new Mock<ISecurityService>();
            securityService.Setup(x => x.CurrentPrincipalName).Returns("Test User");

            return new DefaultMediaFileService(storageService.Object, null, null, cmsConfiguration.Object, null, null, urlResolver.Object, securityService.Object);
        }
    }
}
