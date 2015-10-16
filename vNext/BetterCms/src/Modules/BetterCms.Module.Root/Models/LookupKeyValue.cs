using System;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class LookupKeyValue
    {
        public LookupKeyValue()
        {
        }
            
        public LookupKeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}