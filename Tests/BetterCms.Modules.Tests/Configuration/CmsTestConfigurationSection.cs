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
