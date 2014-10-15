using System;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Core.Models
{
    public class ConfigurationKeyValueDescriptor
    {
        public ConfigurationKeyValueDescriptor()
        {
            TakenFrom = TakenFrom.ModuleDescriptor;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public int Priority { get; set; }

        public Func<string> Title { get; set; }

        //public Action<ICmsConfiguration, object> Map;

        public OptionType Type { get; set; }

        public TakenFrom TakenFrom { get; set; }
    }
}
