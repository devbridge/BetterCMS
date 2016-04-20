// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemStorageServiceTest.cs" company="Devbridge Group LLC">
//
// Copyright (C) 2015,2016 Devbridge Group LLC
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
//
// Website: https://www.bettercms.com
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            var storageService = new FileSystemStorageService();
            string filePath = Path.Combine(@"C:\", "temp", Guid.NewGuid().ToString().Replace("-", string.Empty) + ".test");
            var fileUri = new Uri(filePath);
            storageService.ObjectExists(fileUri);
        }

        [Test]
        public void Should_Throw_StorageException_If_Given_Uri_Is_Not_Of_File_Scheme()
        {
            var httpUri = new Uri("http://www.google.com");
            var storageService = new FileSystemStorageService();

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
                var storageService = new FileSystemStorageService();
                var downloadUri = new Uri(path);
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
