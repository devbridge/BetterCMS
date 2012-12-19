using System;
using System.IO;

using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Services.Storage;

using NUnit.Framework;

namespace BetterCms.Tests.Core.ServiceTests.StorageTests
{
    [TestFixture]
    public class FileSystemStorageServiceTest
    {
        [Test]
        public void Should_Check_That_File_Not_Exists()
        {
            FileSystemStorageService storageService = new FileSystemStorageService();
            string filePath = Path.Combine(@"C:\", "temp", Guid.NewGuid().ToString().Replace("-", string.Empty) + ".test");
            Uri fileUri = new Uri(filePath);
            storageService.ObjectExists(fileUri);
        }

        [Test]
        public void Should_Throw_StorageException_If_Given_Uri_Is_Not_Of_File_Scheme()
        {
            Uri httpUri = new Uri("http://www.google.com");
            FileSystemStorageService storageService = new FileSystemStorageService();

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
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString().Replace("-", string.Empty) + ".tmp");
            const string content = "test content";
            try
            {
                File.WriteAllText(path, content);
                FileSystemStorageService storageService = new FileSystemStorageService();
                Uri downloadUri = new Uri(path);
                var download = storageService.DownloadObject(downloadUri);
                Assert.AreEqual(download.Uri, downloadUri);
                using (TextReader reader = new StreamReader(download.ResponseStream))
                {
                    string contentFromStream = reader.ReadToEnd();
                    Assert.AreEqual(content, contentFromStream);
                }
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [Test]
        public void Should_Upload_Object_Successfully()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test", Guid.NewGuid().ToString().Replace("-", string.Empty) + ".tmp");
            const string content = "test content";

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (TextWriter writer = new StreamWriter(memoryStream))
                    {
                        writer.Write(content);
                        writer.Flush();

                        FileSystemStorageService storageService = new FileSystemStorageService();
                        storageService.UploadObject(
                            new UploadRequest
                                {
                                    Uri = new Uri(path),
                                    InputStream = memoryStream,
                                    CreateDirectory = true
                                });

                        Assert.IsTrue(File.Exists(path));
                        Assert.AreEqual(content, File.ReadAllText(path));
                    }
                }
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }       
    }
}
