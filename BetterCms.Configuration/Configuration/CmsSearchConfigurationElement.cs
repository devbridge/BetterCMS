using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    [ConfigurationCollection(typeof(KeyValueElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class CmsSearchConfigurationElement : ConfigurationElementCollection, ICmsSearchConfiguration
    {    
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