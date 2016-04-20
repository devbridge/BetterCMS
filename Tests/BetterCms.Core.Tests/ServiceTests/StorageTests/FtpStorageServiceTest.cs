// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpStorageServiceTest.cs" company="Devbridge Group LLC">
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
            var cmsConfigurationMock = new Mock<ICmsConfiguration>();
            var storageConfigurationMock = new Mock<ICmsStorageConfiguration>();

            cmsConfigurationMock.Setup(f => f.Storage).Returns(storageConfigurationMock.Object);
            storageConfigurationMock.Setup(f => f.ContentRoot).Returns("ftp://test.ftp.com/cms/content");
            storageConfigurationMock.Setup(f => f.ServiceType).Returns(StorageServiceType.Ftp);
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "FtpRoot"))).Returns("ftp://test.ftp.com");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "FtpUserName"))).Returns("user@test.com");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "FtpPassword"))).Returns("psw123");
            storageConfigurationMock.Setup(f => f.GetValue(It.Is<string>(s => s == "UsePassiveMode"))).Returns("true");

            var httpUri = new Uri("http://www.google.com");
            var storageService = new FtpStorageService(cmsConfigurationMock.Object);

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
