using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;

using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Models;

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
        
        [Test]
        public void Should_Swap_Original_And_Reuploaded_Entities_Correctly()
        {
            var original = TestDataProvider.CreateNewMediaFile();
            var newVersion = TestDataProvider.CreateNewMediaFile();

            var origTitle = original.Title;
            var newVersionTitle = newVersion.Title;

            var cat11 = TestDataProvider.CreateNewCategory(new CategoryTree());
            var cat12 = TestDataProvider.CreateNewCategory(new CategoryTree());
            var cat21 = TestDataProvider.CreateNewCategory(new CategoryTree());

            var mcCat11 = new MediaCategory {Category = cat11, Media = original};
            var mcCat12 = new MediaCategory {Category = cat12, Media = original};
            var mcCat21 = new MediaCategory { Category = cat21, Media = newVersion };

            var tag11 = TestDataProvider.CreateNewTag();
            var tag12 = TestDataProvider.CreateNewTag();
            var tag21 = TestDataProvider.CreateNewTag();

            var mtTag11 = new MediaTag { Tag = tag11, Media = newVersion };
            var mtTag12 = new MediaTag { Tag = tag12, Media = newVersion };
            var mtTag21 = new MediaTag { Tag = tag21, Media = original };

            original.Categories = new List<MediaCategory>();
            newVersion.Categories = new List<MediaCategory>();
            original.MediaTags = new List<MediaTag>();
            newVersion.MediaTags = new List<MediaTag>();

            original.Categories.Add(mcCat11);
            original.Categories.Add(mcCat12);
            newVersion.Categories.Add(mcCat21);

            newVersion.MediaTags.Add(mtTag11);
            newVersion.MediaTags.Add(mtTag12);
            original.MediaTags.Add(mtTag21);

            var service = GetMediaFileService(false);
            service.SwapOriginalMediaWithVersion(original, newVersion);

            // Ensure etity properies are switched
            Assert.AreNotEqual(original.Title, newVersion.Title);
            Assert.AreEqual(origTitle, newVersion.Title);
            Assert.AreEqual(newVersionTitle, original.Title);

            // Ensure original entity is set correctly
            Assert.AreEqual(newVersion.Original, original);

            // Ensure categories are switched correctly
            Assert.AreEqual(original.Categories.Count, 1);
            Assert.AreEqual(newVersion.Categories.Count, 2);

            Assert.IsTrue(newVersion.Categories.Contains(mcCat11));
            Assert.IsTrue(newVersion.Categories.Contains(mcCat12));
            Assert.IsTrue(original.Categories.Contains(mcCat21));
            
            // Ensure tags are switched correctly
            Assert.AreEqual(original.MediaTags.Count, 2);
            Assert.AreEqual(newVersion.MediaTags.Count, 1);

            Assert.IsTrue(original.MediaTags.Contains(mtTag11));
            Assert.IsTrue(original.MediaTags.Contains(mtTag12));
            Assert.IsTrue(newVersion.MediaTags.Contains(mtTag21));
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

            var accessControlService = new Mock<IAccessControlService>();
            accessControlService
                .Setup(x => x.GetAccessLevel(It.IsAny<MediaFile>(), It.IsAny<IPrincipal>(), It.IsAny<bool>()))
                .Returns(AccessLevel.Read);

            var securityService = new Mock<ISecurityService>();
            securityService.Setup(x => x.CurrentPrincipalName).Returns("Test User");

            return new DefaultMediaFileService(storageService.Object, null, null, cmsConfiguration.Object, null,
                null, urlResolver.Object, securityService.Object);
        }
    }
}
