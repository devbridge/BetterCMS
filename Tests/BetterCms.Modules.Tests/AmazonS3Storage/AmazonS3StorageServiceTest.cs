// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmazonS3StorageServiceTest.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.AmazonS3Storage;
using BetterCms.Test.Module.Configuration;

using NUnit.Framework;

namespace BetterCms.Test.Module.AmazonS3Storage
{
    [TestFixture]
    public class AmazonS3torageServiceTest : StorageTestBase
    {
        private const string AmazonAccessKey = "AmazonAccessKey";
        private const string AmazonSecretKey = "AmazonSecretKey";
        private const string AmazonBucketName = "AmazonBucketName";

        [Test]
        public void Should_Upload_Object()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldUploadObject(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Download_Object()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldDownloadObject(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Copy_Object()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldCopyObject(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Download_Url_Unsecured()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration();
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldDownloadUrlUnsecured(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Not_Download_Url_Secured()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration(true);
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldNotDownloadUrlSecured(configuration, amazonStorageService);
        }

        [Test]
        public void Should_Download_Url_Secured()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration(true);
            var amazonStorageService = new AmazonS3StorageService(configuration);

            ShouldDownloadUrl(configuration, amazonStorageService);
        }
        
        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Should_Fail_Timeout()
        {
            Assert.Ignore("TODO: Fix when possible.");

            var configuration = MockConfiguration(true);
            var amazonStorageService = new AmazonS3StorageService(configuration);
            amazonStorageService.Timeout = 1;

            ShouldUploadObject(configuration, amazonStorageService, false);
        }

        /// <summary>
        /// Gets the storage configuration.
        /// </summary>
        /// <param name="serviceSection">The service section.</param>
        /// <returns>
        /// Storage Configuration
        /// </returns>
        protected override ICmsStorageConfiguration GetStorageConfiguration(CmsTestConfigurationSection serviceSection)
        {
            var accessKey = serviceSection.AmazonStorage.GetValue(AmazonAccessKey);
            var secretKey = serviceSection.AmazonStorage.GetValue(AmazonSecretKey);

            if (!string.IsNullOrWhiteSpace(accessKey) && !string.IsNullOrWhiteSpace(secretKey))
            {
                return serviceSection.AmazonStorage;
            }

            accessKey = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_ACCESS_KEY", EnvironmentVariableTarget.Machine);
            secretKey = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_SECRET_KEY", EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrWhiteSpace(accessKey) || !string.IsNullOrWhiteSpace(secretKey))
            {
                var bucketName = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_BUCKET_NAME", EnvironmentVariableTarget.Machine);

                var configuration = new CmsStorageConfigurationElement
                    {
                        ContentRoot = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_CONTENT_ROOT", EnvironmentVariableTarget.Machine),
                        PublicContentUrlRoot = Environment.GetEnvironmentVariable("BETTERCMS_AMAZON_STORAGE_PUBLIC_CONTENT_ROOT", EnvironmentVariableTarget.Machine),
                        ServiceType = StorageServiceType.Auto,
                        ProcessTimeout = serviceSection.AmazonStorage.ProcessTimeout
                    };

                configuration.Add(new KeyValueElement {Key = AmazonAccessKey, Value = accessKey});
                configuration.Add(new KeyValueElement {Key = AmazonSecretKey, Value = secretKey});
                configuration.Add(new KeyValueElement {Key = AmazonBucketName, Value = bucketName});

                return configuration;
            }

            return null;
        }
    }
}
