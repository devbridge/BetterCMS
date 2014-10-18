using System.Collections.Generic;
using System.Linq;

namespace BetterCms.Configuration.Dynamic
{
    public class DynamicModuleElement : ICmsModuleConfiguration
    {
        public DynamicModuleElement()
        {
            ConfigurationValues = new List<ConfigurationKeyValueDescriptor>();
        }

        public string Name { get; set; }

        public string GetValue(string key)
        {
            var setting = ConfigurationValues.FirstOrDefault(p => p.Name.Equals(key));
            return setting != null ? setting.Value : string.Empty;
        }

        public List<ConfigurationKeyValueDescriptor> ConfigurationValues { get; set; }
    }
}
