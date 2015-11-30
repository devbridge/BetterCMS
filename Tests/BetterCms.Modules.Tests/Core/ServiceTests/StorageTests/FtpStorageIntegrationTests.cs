// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpStorageIntegrationTests.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.Services.Storage;

using NUnit.Framework;

namespace BetterCms.Test.Module.Core.ServiceTests.StorageTests
{
    [TestFixture]
    public class FtpStorageIntegrationTests : StorageTestBase
    {
        private const string FtpUserName = "FtpUserName";
        private const string FtpPassword = "FtpPassword";
        private const string FtpUsePassiveMode = "UsePassiveMode";
        private const string FtpRootUrl = "FtpRoot";

        [Test]
        public void Should_Upload_Object()
        {
            var configuration = MockConfiguration();
            var ftpStorageService = new FtpStorageService(configuration);

            ShouldUploadObject(configuration, ftpStorageService);
        }

        [Test]
        public void Should_Download_Object()
        {
            var configuration = MockConfiguration();
            var ftpStorageService = new FtpStorageService(configuration);

            ShouldDownloadObject(configuration, ftpStorageService);
        }

        [Test]
        public void Should_Copy_Object()
        {
            var configuration = MockConfiguration();
            var ftpStorageService = new FtpStorageService(configuration);

            ShouldCopyObject(configuration, ftpStorageService);
        }

        /// <summary>
        /// Gets the storage configuration.
        /// </summary>
        /// <param name="serviceSection">The service section.</param>
        /// <returns>
        /// Storage Configuration
        /// </returns>
        protected override ICmsStorageConfiguration GetStorageConfiguration(Configuration.CmsTestConfigurationSection serviceSection)
        {
            var userName = serviceSection.FtpStorage.GetValue(FtpUserName);
            var password = serviceSection.FtpStorage.GetValue(FtpPassword);

            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                return serviceSection.FtpStorage;
            }

            userName = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_USER_NAME", EnvironmentVariableTarget.Machine);
            password = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_PASSWORD", EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrWhiteSpace(userName) || !string.IsNullOrWhiteSpace(password))
            {
                var usePassiveMode = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_USE_PASSIVE_MODE", EnvironmentVariableTarget.Machine);
                var ftpRoot = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_FTP_ROOT", EnvironmentVariableTarget.Machine);

                var configuration = new CmsStorageConfigurationElement
                {
                    ContentRoot = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_CONTENT_ROOT", EnvironmentVariableTarget.Machine),
                    PublicContentUrlRoot = Environment.GetEnvironmentVariable("BETTERCMS_FTP_STORAGE_CONTENT_ROOT_URL", EnvironmentVariableTarget.Machine),
                    ServiceType = StorageServiceType.Ftp
                };

                configuration.Add(new KeyValueElement { Key = FtpUserName, Value = userName });
                configuration.Add(new KeyValueElement { Key = FtpPassword, Value = password });
                configuration.Add(new KeyValueElement { Key = FtpUsePassiveMode, Value = usePassiveMode });
                configuration.Add(new KeyValueElement { Key = FtpRootUrl, Value = ftpRoot });

                return configuration;
            }

            return null;
        }
    }
}
