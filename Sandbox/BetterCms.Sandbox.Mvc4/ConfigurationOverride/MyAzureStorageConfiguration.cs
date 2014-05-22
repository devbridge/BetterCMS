using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Configuration;

using Microsoft.WindowsAzure.Storage;


namespace BetterCms.Sandbox.Mvc4.ConfigurationOverride
{
    public class MyAzureStorageConfiguration : ICmsStorageConfiguration
    {
        private readonly Dictionary<String, String> configs;
        private TimeSpan? processTimeout;


        public MyAzureStorageConfiguration()
        {
            var connectionString = ConfigurationContext.Current.GetStorageConnectionString();
            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);

            // fucking jiggery-pokery because SRP and Interface Segregation Principles are not adhered to.
            configs = new Dictionary<string, string>
            {
                { "AzureAccountName", cloudStorageAccount.Credentials.AccountName },
                { "AzureSecondaryKey", Convert.ToBase64String(cloudStorageAccount.Credentials.ExportKey()) },
                { "AzureTokenExpiryTime", "00:10" },
                { "AzureUseHttps", "true" }, // why would you not use HTTPS??
                { "AzureContainerName", ConfigurationContext.Current.GetCmsStorageContainerName() },
                { "AzureSecuredContainerName", ConfigurationContext.Current.GetCmsStorageContainerName() },
            };
        }

        public string GetValue(string key)
        {
            if (configs.ContainsKey(key))
            {
                return configs[key];
            }
            throw new Exception(String.Format("CMS Azure Storage Configuration dictionary does not have required key: {0}", key));
        }


        // why do you even want to expose this path?
        // file paths are responsibility of storage service.
        // Service gets a request 
        public string PublicContentUrlRoot
        {
            get
            {
                // outdated and inefficient way to do these things
                var connectionString = ConfigurationContext.Current.GetStorageConnectionString();
                var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                var container = cloudStorageAccount.CreateCloudBlobClient().GetContainerReference(ConfigurationContext.Current.GetCmsStorageContainerName());
                var containerUrl = container.Uri.ToString();
                return containerUrl;
            }
            set
            {
                throw new Exception("MyAzureStorageConfiguration Unalbe to set PublicContentUrlRoot.");
            }
        }

        public string ContentRoot
        {
            get
            {
                return PublicContentUrlRoot;
            }
            set
            {
                throw new Exception("MyAzureStorageConfiguration Unalbe to set ContentRoot.");
            }
        }

        public string PublicSecuredContentUrlRoot
        {
            get
            {
                return PublicContentUrlRoot;
            }
            set
            {
                throw new Exception("MyAzureStorageConfiguration Unalbe to set PublicSecuredContentUrlRoot.");
            }
        }


        public string SecuredContentRoot
        {
            get
            {
                return PublicContentUrlRoot;
            }
            set
            {
                throw new Exception("MyAzureStorageConfiguration Unalbe to set PublicSecuredContentUrlRoot.");
            }
        }


        public StorageServiceType ServiceType
        {
            get
            {
                return StorageServiceType.Custom;
            }
            set
            {
                throw new ApplicationException("Can't change the type of the provider");
            }
        }


        public TimeSpan ProcessTimeout
        {
            get
            {
                if (processTimeout.HasValue)
                {
                    return processTimeout.Value;
                }
                return TimeSpan.FromSeconds(10);
            }
            set
            {
                processTimeout = value;
            }
        }
    }
}