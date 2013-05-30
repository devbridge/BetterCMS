using System;
using System.IO;

using BetterCms.Configuration;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.WindowsAzureStorage;

using NUnit.Framework;

namespace BetterCms.Test.Module.WindowsAzureStorage
{
    [TestFixture]
    public class WindowsAzureStorageServiceTest : StorageTestBase
    {
        private const string AzureAccountName = "AzureAccountName";
        private const string AzureSecondaryKey = "AzureSecondaryKey";
        private const string AzureContainerName = "AzureContainerName";
        private const string AzureUseHttps = "AzureUseHttps";

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
            var azureClient = CreateService();

            var uri = new Uri("http://bettercms.blob.core.windows.net/temp/47.jpg");
            GetUploadRequest(uri, azureClient);
            bool exits = azureClient.ObjectExists(uri);

            bool notExist = azureClient.ObjectExists(new Uri("http://bettercms.blob.core.windows.net/temp/47      .jpg"));

            Assert.IsTrue(exits);
            Assert.IsFalse(notExist);
        }

        [Test]
        // [Ignore]
        public void Should_Upload_Object()
        {
            var azureClient = CreateService();           
           
            var uri = new Uri("http://bettercms.blob.core.windows.net/temp/Koala2.jpg");

            GetUploadRequest(uri, azureClient);

            Assert.IsTrue(azureClient.ObjectExists(uri));
            azureClient.RemoveObject(uri);
        }

        [Test]
        [Ignore]
        public void Should_Download_Object()
        {
            var azureClient = CreateService();
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
            var azureClient = CreateService();
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
            var azureClient = CreateService();
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
            var azureClient = CreateService();
            GetUploadRequest(uri, azureClient);
            GetUploadRequest(uri1, azureClient);
            azureClient.RemoveFolder(uri);
            Assert.IsFalse(azureClient.ObjectExists(uri));
            Assert.IsFalse(azureClient.ObjectExists(uri1));
        }

        private WindowsAzureStorageService CreateService()
        {
            var configuration = MockConfiguration();
            return new WindowsAzureStorageService(configuration);
        }

        protected override ICmsStorageConfiguration GetStorageConfiguration(Configuration.CmsTestConfigurationSection serviceSection)
        {
            var accountName = serviceSection.AmazonStorage.GetValue(AzureAccountName);
            var secretKey = serviceSection.AmazonStorage.GetValue(AzureSecondaryKey);

            if (!string.IsNullOrWhiteSpace(accountName) && !string.IsNullOrWhiteSpace(secretKey))
            {
                return serviceSection.AmazonStorage;
            }

            accountName = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_ACCOUNT_KEY", EnvironmentVariableTarget.User);
            secretKey = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_SECONDARY_KEY", EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(accountName) || !string.IsNullOrWhiteSpace(secretKey))
            {
                var containerName = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_CONTAINER_NAME", EnvironmentVariableTarget.User);
                var useHttps = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_USE_HTTPS", EnvironmentVariableTarget.User);

                var configuration = new CmsStorageConfigurationElement
                {
                    ContentRoot = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_CONTENT_ROOT", EnvironmentVariableTarget.User),
                    ServiceType = StorageServiceType.Auto
                };

                configuration.Add(new KeyValueElement { Key = AzureAccountName, Value = accountName });
                configuration.Add(new KeyValueElement { Key = AzureSecondaryKey, Value = secretKey });
                configuration.Add(new KeyValueElement { Key = AzureContainerName, Value = containerName });
                if (!string.IsNullOrWhiteSpace(useHttps))
                {
                    configuration.Add(new KeyValueElement { Key = AzureUseHttps, Value = useHttps });
                }

                return configuration;
            }

            return null;
        }
    }
}
