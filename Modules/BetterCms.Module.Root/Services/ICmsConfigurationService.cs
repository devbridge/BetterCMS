using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Services
{
    public interface ICmsConfigurationService
    {
        ConfigurationKeyValueDescriptor GetKeyValue(ConfigurationKeyValueDescriptor keyValueDescriptor);

        IEnumerable<ConfigurationKeyValueDescriptor> GetKeyValuesForModule(IEnumerable<ConfigurationKeyValueDescriptor> keyValueDescriptors);
    }
}
