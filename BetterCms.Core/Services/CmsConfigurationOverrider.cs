using System.Collections.Generic;
using System.Linq;

using BetterCms.Configuration.Dynamic;
using BetterCms.Core.Models;
using BetterCms.Core.Modules;

namespace BetterCms.Core.Services
{
    public static class CmsConfigurationOverrider
    {
        public static void Override(this ModuleDescriptor descriptor, ISettingsService settingsService,
            ConfigurationKeyValueDescriptor keyValueDescriptor, ICmsConfiguration configuration)
        {

            if (ValueExistsInAppSettings(descriptor.Name, keyValueDescriptor, configuration))
            {
                return;
            }

            if (ValueExistsInDatabase(descriptor, settingsService, keyValueDescriptor, configuration))
            {
                return;
            }

            UpdateWithDeafultValue(descriptor.Name, keyValueDescriptor, configuration);
        }

        private static bool ValueExistsInAppSettings(string moduleName, ConfigurationKeyValueDescriptor keyValueDescriptor, ICmsConfiguration configuration)
        {
            var appSettingsSection = CmsContext.AppSettingsConfiguration;
            var appSettingModulePropertyName = string.Format("bcms_{0}_{1}", moduleName, keyValueDescriptor.Key);
            var property = appSettingsSection.Settings[appSettingModulePropertyName];

            if (property == null)
            {
                return false;
            }

            var moduleConfiguration = configuration.Modules.First(m => m.Name.Equals(moduleName));
            if (moduleConfiguration != null)
            {
                moduleConfiguration.SetValue(keyValueDescriptor.Key, property.Value);
            }
            else
            {
                configuration.Modules.Add(
                    new DynamicModule
                    {
                        Name = moduleName,
                        KeyValues = new List<DynamicKeyValue> { new DynamicKeyValue { Key = keyValueDescriptor.Key, Value = property.Value } }
                    });
            }

            return true;
        }

        private static bool ValueExistsInDatabase(ModuleDescriptor descriptor, ISettingsService settingsService,
            ConfigurationKeyValueDescriptor keyValueDescriptor, ICmsConfiguration configuration)
        {
            var moduleSettings = settingsService.GetModuleSettings(descriptor.Id);

            if (moduleSettings == null)
            {
                return false;
            }

            var property = moduleSettings.First(s => s.Name.Equals(keyValueDescriptor.Key));
            if (property == null)
            {
                return false;
            }

            var moduleConfiguration = configuration.Modules.First(m => m.Name.Equals(descriptor.Name));
            if (moduleConfiguration != null)
            {
                moduleConfiguration.SetValue(keyValueDescriptor.Key, keyValueDescriptor.Value);
            }
            else
            {
                configuration.Modules.Add(
                    new DynamicModule
                    {
                        Name = descriptor.Name,
                        KeyValues = new List<DynamicKeyValue> { new DynamicKeyValue { Key = keyValueDescriptor.Key, Value = property.Value } }
                    });
            }

            return true;
        }

        private static void UpdateWithDeafultValue(string moduleName, ConfigurationKeyValueDescriptor keyValueDescriptor, ICmsConfiguration configuration)
        {
            var moduleConfiguration = configuration.Modules.First(m => m.Name.Equals(moduleName));
            if (moduleConfiguration != null)
            {
                moduleConfiguration.SetValue(keyValueDescriptor.Key, keyValueDescriptor.Value);
            }
            else
            {
                configuration.Modules.Add(
                    new DynamicModule
                    {
                        Name = moduleName,
                        KeyValues = new List<DynamicKeyValue> { new DynamicKeyValue { Key = keyValueDescriptor.Key, Value = keyValueDescriptor.Value } }
                    });
            }
        }
    }
}
