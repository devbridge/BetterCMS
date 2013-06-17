using System;

using BetterCms.Configuration;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Services.Storage;

using Moq;

using NUnit.Framework;

namespace BetterCms.Tests.Core.ServiceTests.StorageTests
{
    [TestFixture]
    public class FtpStorageServiceTest
    {
        [Test]
        public void Should_Throw_StorageException_If_Given_Uri_Is_Not_Of_File_Scheme()
        {
            Mock<ICmsConfiguration> cmsConfigurationMock = new Mock<ICmsConfiguration>();
            Mock<ICmsStorageConfiguration> storageConfigurationMock = new Mock<ICmsStorageConfiguration>();

            cmsConfigurationMock.Setup(f => f.Storage).Returns(storageConfigurationMock.Object);
            storageConfigurationMock.Setup(f => f.ContentRoot).Returns("ftp://test.ftp.com/cms/content");
            storageConfigurationMock.Setup(f => f.ServiceType).Returns(StorageServiceType.Ftp);
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "FtpRoot"))).Returns("ftp://test.ftp.com");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "FtpUserName"))).Returns("user@test.com");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "FtpPassword"))).Returns("psw123");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "UsePassiveMode"))).Returns("true");

            Uri httpUri = new Uri("http://www.google.com");
            FtpStorageService storageService = new FtpStorageService(cmsConfigurationMock.Object);

            var ex1 = Assert.Throws<StorageException>(() => storageService.ObjectExists(httpUri));
            var ex2 = Assert.Throws<StorageException>(() => storageService.CopyObject(httpUri, httpUri));
            var ex3 = Assert.Throws<StorageException>(() => storageService.DownloadObject(httpUri));
            var ex4 = Assert.Throws<StorageException>(() => storageService.UploadObject(new UploadRequest { Uri = httpUri }));

            Assert.IsTrue(ex1.Message.StartsWith("An Uri scheme") && ex1.Message.Contains("can't be processed with a"));
            Assert.IsTrue(ex2.Message.StartsWith("An Uri scheme") && ex2.Message.Contains("can't be processed with a"));
            Assert.IsTrue(ex3.Message.StartsWith("An Uri scheme") && ex3.Message.Contains("can't be processed with a"));
            Assert.IsTrue(ex4.Message.StartsWith("An Uri scheme") && ex4.Message.Contains("can't be processed with a"));
        }
       
    }
}
