// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CmsTestConfigurationSection.cs" company="Devbridge Group LLC">
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
using System.Configuration;

using BetterCms.Configuration;

namespace BetterCms.Test.Module.Configuration
{
    public class CmsTestConfigurationSection : ConfigurationSection
    {
        public const string StorageSectionName = "storage";

        private const string AmazonStorageNode = "amazonStorage";
        private const string AzureStorageNode = "azureStorage";
        private const string FtpStorageNode = "ftpStorage";

        [ConfigurationProperty(AmazonStorageNode, IsRequired = true)]
        public CmsStorageConfigurationElement AmazonStorage
        {
            get { return (CmsStorageConfigurationElement)this[AmazonStorageNode]; }
            set { this[AmazonStorageNode] = value; }
        }
        
        [ConfigurationProperty(AzureStorageNode, IsRequired = true)]
        public CmsStorageConfigurationElement AzureStorage
        {
            get { return (CmsStorageConfigurationElement)this[AzureStorageNode]; }
            set { this[AzureStorageNode] = value; }
        }
        
        [ConfigurationProperty(FtpStorageNode, IsRequired = true)]
        public CmsStorageConfigurationElement FtpStorage
        {
            get { return (CmsStorageConfigurationElement)this[FtpStorageNode]; }
            set { this[FtpStorageNode] = value; }
        }
    }
}
