using System;
using System.Configuration;
using System.Globalization;

namespace BetterCms.Configuration
{
    [ConfigurationCollection(typeof(KeyValueElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class CmsStorageConfigurationElement : ConfigurationElementCollection, ICmsStorageConfiguration
    {
        private const string ContentRootAttribute = "contentRoot";
        
        private const string SecuredContentRootAttribute = "securedContentRoot";

        private const string PublicContentUrlRootAttribute = "contentRootUrl";
        
        private const string PublicSecuredContentUrlRootAttribute = "securedContentRootUrl";

        private const string ServiceTypeAttribute = "serviceType";

        private const string ProcessTimeoutAttribute = "processTimeout";

        private const string MaximumFileNameLengthAttribute = "maxFileNameLength";

        [ConfigurationProperty(ContentRootAttribute, IsRequired = true)]
        public string ContentRoot
        {
            get { return ParseEnvironmentValue(Convert.ToString(this[ContentRootAttribute])); }
            set { this[ContentRootAttribute] = value; }
        }

        [ConfigurationProperty(PublicContentUrlRootAttribute, IsRequired = false, DefaultValue = null)]
        public string PublicContentUrlRoot
        {
            get
            {
                string urlRoot = (string)this[PublicContentUrlRootAttribute];                
                if (string.IsNullOrEmpty(urlRoot))
                {
                    return ContentRoot;
                }

                return ParseEnvironmentValue(urlRoot);
            }
            set
            {
                this[PublicContentUrlRootAttribute] = value;
            }
        }

        [ConfigurationProperty(SecuredContentRootAttribute, IsRequired = false, DefaultValue = null)]
        public string SecuredContentRoot
        {
            get
            {
                string urlRoot = (string)this[SecuredContentRootAttribute];
                if (string.IsNullOrEmpty(urlRoot))
                {
                    return ContentRoot;
                }

                return ParseEnvironmentValue(urlRoot);
            }
            set
            {
                this[SecuredContentRootAttribute] = value;
            }
        }

        [ConfigurationProperty(PublicSecuredContentUrlRootAttribute, IsRequired = false, DefaultValue = null)]
        public string PublicSecuredContentUrlRoot
        {
            get
            {
                string urlRoot = (string)this[PublicSecuredContentUrlRootAttribute];                
                if (string.IsNullOrEmpty(urlRoot))
                {
                    return PublicContentUrlRootAttribute;
                }

                return ParseEnvironmentValue(urlRoot);
            }
            set
            {
                this[PublicSecuredContentUrlRootAttribute] = value;
            }
        }

        [ConfigurationProperty(ServiceTypeAttribute, IsRequired = false, DefaultValue = StorageServiceType.Ftp)]
        public StorageServiceType ServiceType
        {
            get { return (StorageServiceType)this[ServiceTypeAttribute]; }
            set { this[ServiceTypeAttribute] = value; }
        }

        [ConfigurationProperty(ProcessTimeoutAttribute, IsRequired = false, DefaultValue = "00:05:00")]
        public TimeSpan ProcessTimeout
        {
            get { return (TimeSpan)this[ProcessTimeoutAttribute]; }
            set { this[ProcessTimeoutAttribute] = value; }
        }

        [ConfigurationProperty(MaximumFileNameLengthAttribute, IsRequired = false, DefaultValue = 0)]
        public int MaximumFileNameLength
        {
            get
            {
                int length;
                Int32.TryParse(this[MaximumFileNameLengthAttribute].ToString(), out length);
                
                return length;
            }
            set { this[MaximumFileNameLengthAttribute] = value; }
        }

        public KeyValueElement this[int index]
        {
            get
            {
                return (KeyValueElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new KeyValueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as KeyValueElement).Key;
        }

        public string GetValue(string key)
        {
            var element = (KeyValueElement)BaseGet(key);
            return element == null ? null : ParseEnvironmentValue(element.Value);
        }

        public void Add(KeyValueElement element)
        {
            BaseAdd(element);
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