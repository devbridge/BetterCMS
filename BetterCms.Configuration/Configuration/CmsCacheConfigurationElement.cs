using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    [ConfigurationCollection(typeof(KeyValueElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class CmsCacheConfigurationElement : ConfigurationElementCollection, ICmsCacheConfiguration
    {
        private const string CacheTypeAttribute = "cacheType";

        private const string EnabledAttribute = "enabled";

        private const string TimeoutAttribute = "timeout";

        [ConfigurationProperty(EnabledAttribute, IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this[EnabledAttribute]; }
            set { this[EnabledAttribute] = value; }
        }

        [ConfigurationProperty(TimeoutAttribute, IsRequired = false, DefaultValue = "0:10:0")]
        public TimeSpan Timeout
        {
            get { return (TimeSpan)this[TimeoutAttribute]; }
            set { this[TimeoutAttribute] = value; }
        }

        [ConfigurationProperty(CacheTypeAttribute, IsRequired = false, DefaultValue = CacheServiceType.Auto)]
        public CacheServiceType CacheType
        {
            get { return (CacheServiceType)this[CacheTypeAttribute]; }
            set { this[CacheTypeAttribute] = value; }
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
            return element == null ? null : element.Value;
        }
    }
}