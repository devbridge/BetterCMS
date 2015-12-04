// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowsAzureStorageServiceTest.cs" company="Devbridge Group LLC">
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

        [Test]
        public void Should_Upload_Object()
        {
            Assert.Ignore("TODO: Fix when possible.");
            var configuration = MockConfiguration();
            var azureClient = new WindowsAzureStorageService(configuration);

            ShouldUploadObject(configuration, azureClient);
        }

        [Test]
        public void Should_Download_Object()
        {
            Assert.Ignore("TODO: Fix when possible.");
            var configuration = MockConfiguration();
            var azureClient = new WindowsAzureStorageService(configuration);

            ShouldDownloadObject(configuration, azureClient);
        }

        [Test]
        public void Should_Copy_Object()
        {
            Assert.Ignore("TODO: Fix when possible.");
            var configuration = MockConfiguration();
            var azureClient = new WindowsAzureStorageService(configuration);

            ShouldCopyObject(configuration, azureClient);
        }

        [Test]
        public void Should_Download_Url_Unsecured()
        {
            Assert.Ignore("TODO: Fix when possible.");
            var configuration = MockConfiguration();
            var azureClient = new WindowsAzureStorageService(configuration);

            ShouldDownloadUrlUnsecured(configuration, azureClient);
        }
        
        [Test]
        public void Should_Not_Download_Url_Secured()
        {
            Assert.Ignore("TODO: Fix when possible: need different containers (secured and unsecured).");

            var configuration = MockConfiguration(true);
            var azureClient = new WindowsAzureStorageService(configuration);

            ShouldNotDownloadUrlSecured(configuration, azureClient);
        }
        
        [Test]
        public void Should_Download_Url_Secured()
        {
            Assert.Ignore("TODO: Fix when possible.");
            var configuration = MockConfiguration(true);
            var azureClient = new WindowsAzureStorageService(configuration);

            ShouldDownloadUrl(configuration, azureClient);
        }

        [Test]
        [ExpectedException(typeof(BetterCms.Core.Exceptions.Service.StorageException))]
        public void Should_Fail_Timeout()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration(true);
            var azureClient = new WindowsAzureStorageService(configuration);
            azureClient.Timeout = new TimeSpan(0, 0, 0, 0, 1);

            ShouldUploadObject(configuration, azureClient, false);
        }

        protected override ICmsStorageConfiguration GetStorageConfiguration(Configuration.CmsTestConfigurationSection serviceSection)
        {
            var accountName = serviceSection.AzureStorage.GetValue(AzureAccountName);
            var secretKey = serviceSection.AzureStorage.GetValue(AzureSecondaryKey);

            if (!string.IsNullOrWhiteSpace(accountName) && !string.IsNullOrWhiteSpace(secretKey))
            {
                return serviceSection.AzureStorage;
            }

            accountName = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_ACCOUNT_KEY", EnvironmentVariableTarget.Machine);
            secretKey = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_SECONDARY_KEY", EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrWhiteSpace(accountName) || !string.IsNullOrWhiteSpace(secretKey))
            {
                var containerName = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_CONTAINER_NAME", EnvironmentVariableTarget.Machine);
                var useHttps = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_USE_HTTPS", EnvironmentVariableTarget.Machine);

                var configuration = new CmsStorageConfigurationElement
                {
                    ContentRoot = Environment.GetEnvironmentVariable("BETTERCMS_AZURE_STORAGE_CONTENT_ROOT", EnvironmentVariableTarget.Machine),
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
