using System;

using BetterCms.Configuration;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Services.Storage;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.AmazonS3Storage
{
    [TestFixture]
    public class FtpStorageServiceTest
    {
        [Test]
        public void Should_Check_That_File_Not_Exists()
        {
           Assert.Ignore();
        }

        [Test]
        public void Should_Throw_StorageException_If_Given_Uri_Is_Not_Of_File_Scheme()
        {
            Mock<ICmsConfiguration> cmsConfigurationMock = new Mock<ICmsConfiguration>();
            Mock<ICmsStorageConfiguration> storageConfigurationMock = new Mock<ICmsStorageConfiguration>();

            cmsConfigurationMock.Setup(f => f.Storage).Returns(storageConfigurationMock.Object);
            storageConfigurationMock.Setup(f => f.ContentRoot).Returns("ftp://test.ftp.com/cms/content");
            storageConfigurationMock.Setup(f => f.ServiceType).Returns(StorageServiceType.Auto);
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "AmazonAccessKey"))).Returns("aaa");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "AmazonSecretKey"))).Returns("bbb");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "AmazonBucketName"))).Returns("bucket");
            
            Uri httpUri = new Uri(@"c:\www\document.html");
            FtpStorageService storageService = new FtpStorageService(cmsConfigurationMock.Object);

            var ex1 = Assert.Throws<StorageException>(() => storageService.ObjectExists(httpUri));
            var ex2 = Assert.Throws<StorageException>(() => storageService.CopyObject(httpUri, httpUri));
            var ex3 = Assert.Throws<StorageException>(() => storageService.DownloadObject(httpUri));
            var ex4 = Assert.Throws<StorageException>(() => storageService.UploadObject(new UploadRequest { Uri = httpUri }));

            Assert.IsTrue(ex1.Message.StartsWith("An Uri scheme") && ex1.Message.Contains("can't be processed with a"));
            Assert.IsTrue(ex2.Message.StartsWith("An Uri scheme") && ex1.Message.Contains("can't be processed with a"));
            Assert.IsTrue(ex3.Message.StartsWith("An Uri scheme") && ex1.Message.Contains("can't be processed with a"));
            Assert.IsTrue(ex4.Message.StartsWith("An Uri scheme") && ex1.Message.Contains("can't be processed with a"));
        }

        [Test]
        public void Should_Download_Object_Successfully()
        {
            Assert.Ignore();
        }

        [Test]
        public void Should_Upload_Object_Successfully()
        {
            Assert.Ignore();
        }        
    }
}
