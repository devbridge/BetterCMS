using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterCms.Configuration
{
    public class CmsStorageConfigurationElement
//: ConfigurationElementCollection, ICmsStorageConfiguration
    {
        public CmsStorageConfigurationElement()
        {
            Options = new List<KeyValueElement>();
        }

        public string ContentRoot { get; set; }

        private string publicContentUrlRoot;
        
        public string PublicContentUrlRoot
        {
            get
            {
                return string.IsNullOrEmpty(publicContentUrlRoot) ? ContentRoot : ParseEnvironmentValue(publicContentUrlRoot);
            }
            set
            {
                publicContentUrlRoot = value;
            }
        }

        private string securedContentRoot;

        public string SecuredContentRoot
        {
            get
            {
                return string.IsNullOrEmpty(securedContentRoot) ? ContentRoot : ParseEnvironmentValue(securedContentRoot);
            }
            set
            {
                securedContentRoot = value;
            }
        }

        private string publicSecuredContentUrlRoot;

        public string PublicSecuredContentUrlRoot
        {
            get
            {
                return string.IsNullOrEmpty(publicSecuredContentUrlRoot) ? ContentRoot : ParseEnvironmentValue(publicSecuredContentUrlRoot);
            }
            set
            {
                publicSecuredContentUrlRoot = value;
            }
        }

        public StorageServiceType ServiceType { get; set; } = StorageServiceType.Ftp;

        public TimeSpan ProcessTimeout { get; set; } = TimeSpan.Parse("00:05:00");

        public int MaximumFileNameLength { get; set; }
        
        public string TrashFolder { get; set; }

        public IList<KeyValueElement> Options { get; set; }

        public string GetValue(string key)
        {
            return Options.FirstOrDefault(x => x.Key == key)?.Value;
        }

        private string ParseEnvironmentValue(string value)
        {
            if (value != null && value.StartsWith("[") && value.EndsWith("]") && value.Length > 2)
            {
                var envKey = value.Substring(1, value.Length - 2);

                return Environment.GetEnvironmentVariable(envKey, EnvironmentVariableTarget.Machine);
            }

            return value;
        }
    }
}