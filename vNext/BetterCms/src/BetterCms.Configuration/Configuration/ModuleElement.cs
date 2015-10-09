using System.Collections.Generic;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class ModuleElement //: ConfigurationElementCollection
    {
        public ModuleElement()
        {
            Options = new List<KeyValueElement>();
        }

        public string Name { get; set; }

        public IList<KeyValueElement> Options { get; set; }

        //public KeyValueElement this[int index]
        //{
        //    get
        //    {
        //        return (KeyValueElement)BaseGet(index);
        //    }
        //    set
        //    {
        //        if (BaseGet(index) != null)
        //        {
        //            BaseRemoveAt(index);
        //        }
        //        BaseAdd(index, value);
        //    }
        //}

        //protected override ConfigurationElement CreateNewElement()
        //{
        //    return new KeyValueElement();
        //}

        //protected override object GetElementKey(ConfigurationElement element)
        //{
        //    return (element as KeyValueElement).Key;
        //}

        //public string GetValue(string key)
        //{
        //    var element = (KeyValueElement)BaseGet(key);
        //    return element == null ? null : element.Value;
        //}

        //public void Add(KeyValueElement element)
        //{
        //    BaseAdd(element);
        //}
    }
}
