using System.Collections.Generic;
using System.Linq;

namespace BetterCms.Configuration.Dynamic
{
    public class DynamicModule : ICmsModuleConfiguration
    {
        public string Name { get; set; }

        public string GetValue(string key)
        {
            var keyValuePair = KeyValues.FirstOrDefault(p => p.Key.Equals(key));
            return keyValuePair != null ? keyValuePair.Value : string.Empty;
        }

        public void SetValue(string key, string value)
        {
            var keyValuePair = KeyValues.FirstOrDefault(p => p.Key.Equals(key));
            if (keyValuePair != null)
            {
                keyValuePair.Value = value;
            }
            else
            {
                KeyValues.Add(new DynamicKeyValue
                {
                    Key = key,
                    Value = value
                });
            }
        }

        public List<DynamicKeyValue> KeyValues { private get; set; }
    }
}
