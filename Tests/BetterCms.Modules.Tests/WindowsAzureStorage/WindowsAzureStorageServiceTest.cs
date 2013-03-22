using System;
using System.Collections.Specialized;
using System.IO;

using BetterCms.Configuration;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.AmazonS3Storage;


using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.WindowsAzureStorage
{
    [TestFixture]
    public class WindowsAzureStorageServiceTest
    {
        private ICmsConfiguration CreateCmsConfigurationMock()
        {
            Mock<ICmsConfiguration> cmsConfigurationMock = new Mock<ICmsConfiguration>();
            Mock<ICmsStorageConfiguration> storageConfigurationMock = new Mock<ICmsStorageConfiguration>();
            cmsConfigurationMock.Setup(f => f.Storage).Returns(storageConfigurationMock.Object);
            storageConfigurationMock.Setup(f => f.ContentRoot).Returns("ftp://test.ftp.com/cms/content");
            storageConfigurationMock.Setup(f => f.ServiceType).Returns(StorageServiceType.Auto);
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "AzureAccountName"))).Returns("accountName");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "AzureSecondaryKey"))).Returns("password");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "AzureUseHttps"))).Returns("true");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "AzureContainerName"))).Returns("container");

            return cmsConfigurationMock.Object;
        }

        private void GetUploadRequest(Uri uri, IStorageService azure)
        {
            var request = new UploadRequest();                      

            using (var file = File.OpenRead(@"C:\Users\Vytautas\Pictures\Koala.jpg"))
            {
                request.InputStream = file;
                request.Uri = uri;
                request.CreateDirectory = true;
                azure.UploadObject(request);                
            }           
        }

        [Test]
        [Ignore]
        public void Should_Check_If_Object_Exists()
        {
            ICmsConfiguration config = CreateCmsConfigurationMock();

            var azureClient = new WindowsAzureStorageService(config);
            var uri = new Uri("http://bettercms.blob.core.windows.net/temp/47.jpg");
            GetUploadRequest(uri, azureClient);
            bool exits = azureClient.ObjectExists(uri);

            bool notExist = azureClient.ObjectExists(new Uri("http://bettercms.blob.core.windows.net/temp/47      .jpg"));

            Assert.IsTrue(exits);
            Assert.IsFalse(notExist);
        }

        [Test]
        [Ignore]
        public void Should_Upload_Object()
        {
            var azureClient = new WindowsAzureStorageService(CreateCmsConfigurationMock());           
           
            var uri = new Uri("http://bettercms.blob.core.windows.net/temp/Koala2.jpg");

            GetUploadRequest(uri, azureClient);

            Assert.IsTrue(azureClient.ObjectExists(uri));
            azureClient.RemoveObject(uri);
        }

        [Test]
        [Ignore]
        public void Should_Download_Object()
        {
            var azureClient = new WindowsAzureStorageService(CreateCmsConfigurationMock());
            var uri = new Uri("http://bettercms.blob.core.windows.net/temp/newFile4.jpg");
            GetUploadRequest(uri, azureClient);
            var file = azureClient.DownloadObject(uri);
            Assert.IsTrue(file.ResponseStream.Length > 0);
            azureClient.RemoveObject(uri);
        }

        [Test]
        [Ignore]
        public void Should_Copy_Object()
        {
            var azureClient = new WindowsAzureStorageService(CreateCmsConfigurationMock());
            var source = new Uri("http://bettercms.blob.core.windows.net/temp/newFile4.jpg");
            GetUploadRequest(source, azureClient);
            var destination = new Uri("http://bettercms.blob.core.windows.net/temp/newFile4 copy.jpg");
            azureClient.CopyObject(new Uri("http://bettercms.blob.core.windows.net/temp/newFile4.jpg"), destination);
            Assert.IsTrue(azureClient.ObjectExists(destination));
            azureClient.RemoveObject(source);
            azureClient.RemoveObject(destination);
        }

        [Test]
        [Ignore]
        public void Should_Remove_Object()
        {

            var azureClient = new WindowsAzureStorageService(CreateCmsConfigurationMock());
            var uri = new Uri("http://bettercms.blob.core.windows.net/temp/newFile4copy.jpg");
            GetUploadRequest(uri, azureClient);
            azureClient.RemoveObject(uri);
            Assert.IsFalse(azureClient.ObjectExists(uri));
        }

        [Test]
        [Ignore]
        public void Should_Remove_Folder()
        {
            var uri = new Uri("http://bettercms.blob.core.windows.net/temp/folder/gifas.gif");
            var uri1 = new Uri("http://bettercms.blob.core.windows.net/temp/folder/gifas1.gif");
            var azureClient = new WindowsAzureStorageService(CreateCmsConfigurationMock());
            GetUploadRequest(uri, azureClient);
            GetUploadRequest(uri1, azureClient);
            azureClient.RemoveFolder(uri);
            Assert.IsFalse(azureClient.ObjectExists(uri));
            Assert.IsFalse(azureClient.ObjectExists(uri1));
        }
    }
}
