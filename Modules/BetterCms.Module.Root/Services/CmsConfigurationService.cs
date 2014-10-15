using System.Collections.Generic;
using System.Configuration;

using BetterCms.Core.Models;
using BetterCms.Core.Services;

namespace BetterCms.Module.Root.Services
{
    public class CmsConfigurationService : ICmsConfigurationService
    {
        private ICmsConfiguration cmsConfiguration;

        private AppSettingsSection appSettingsSection;

        private ISettingsService settingsService;

        public CmsConfigurationService(ICmsConfiguration cmsConfiguration, AppSettingsSection appSettingsSection,
            ISettingsService settingsService)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.appSettingsSection = appSettingsSection;
            this.settingsService = settingsService;
        }

        public ConfigurationKeyValueDescriptor GetKeyValue(ConfigurationKeyValueDescriptor keyValueDescriptor)
        {
            return keyValueDescriptor;
        }

        public IEnumerable<ConfigurationKeyValueDescriptor> GetKeyValuesForModule(IEnumerable<ConfigurationKeyValueDescriptor> keyValueDescriptors)
        {
            return keyValueDescriptors;
        }
    }
}
