using System;

namespace BetterCms.Configuration.Dynamic
{
    public class ConfigurationKeyValueDescriptor
    {
        public ConfigurationKeyValueDescriptor()
        {
            Priority = 1;
            TakenFrom = TakenFrom.ModuleDescriptor;
        }

        public string Value { get; set; }

        public int Priority { get; set; }

        public Func<string> Title { get; set; }

        //public Action<ICmsConfiguration, object> Map;

        public OptionType Type { get; set; }

        public TakenFrom TakenFrom { get; set; }

        public string Name { get; set; }
    }
}
