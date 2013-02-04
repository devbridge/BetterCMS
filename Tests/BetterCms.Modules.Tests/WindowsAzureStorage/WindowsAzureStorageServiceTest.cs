using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using NUnit.Framework;

namespace BetterCms.Test.Module.WindowsAzureStorage
{
    [TestFixture]
    public class WindowsAzureStorageServiceTest
    {
        private CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
/*            
        [Test]
        public void Should_Check_If_Object_Exists()
        {
            var client = cloudStorageAccount.CreateCloudBlobClient();
            client.ParallelOperationThreadCount = 1;

            var exits = client.GetBlobReferenceFromServer(new Uri("http://bettercms.blob.core.windows.net/temp/47.jpg")).Exists();
            bool notExist;

            try
            {
                notExist = client.GetBlobReferenceFromServer(new Uri("http://bettercms.blob.core.windows.net/temp/47__.jpg")).Exists();
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == (int)HttpStatusCode.NotFound)
                {
                    notExist = false;
                }
                else
                {
                    throw;
                }
            }
            
            Assert.IsTrue(exits);
            Assert.IsFalse(notExist);
        }

        [Test]
        public void Should_Upload_Object()
        {
            var client = cloudStorageAccount.CreateCloudBlobClient();
            client.ParallelOperationThreadCount = 1;

            var container = client.GetContainerReference("temp");
            var path = new Uri("http://bettercms.blob.core.windows.net/temp/newFile3.jpg").AbsoluteUri;
            var blob = container.GetBlockBlobReference(path);

            using (var file = File.OpenRead(@"C:\Users\Paulius\Pictures\tg3.png"))
            {
                blob.Properties.ContentType = @"image\jpeg";
                blob.UploadFromStream(file);
            }
        }

        [Test]
        public void Should_Download_Object()
        {
            var client = cloudStorageAccount.CreateCloudBlobClient();
            client.ParallelOperationThreadCount = 1;

            var blob = client.GetBlobReferenceFromServer(new Uri("http://bettercms.blob.core.windows.net/temp/47.jpg"));
            using (var memoryStream = new MemoryStream())
            {
                blob.DownloadToStream(memoryStream);

                Assert.IsTrue(memoryStream.Length > 0);
            }            
        }

        [Test]
        public void T()
        {
            //NameValueCollection 
        }
 * */
    }
}
