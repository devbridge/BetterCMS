using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using BetterCms.Configuration.Dynamic;
using BetterCms.Core.Models;
using BetterCms.Core.Modules;

namespace BetterCms.Core.Services
{
    public static class CmsConfigurationOverrider
    {
        public static void Override(
            this ModuleDescriptor descriptor,
            IEnumerable<ConfigurationKeyValueDescriptor> constantValues,
            ICmsConfiguration cmsConfiguration,
            IEnumerable<KeyValueConfigurationElement> appSettingsValues,
            IEnumerable<Setting> databaseValues)
        {
            DynamicModuleElement cmsModuleConfiguration = (DynamicModuleElement)cmsConfiguration.Modules.FirstOrDefault(m => m.Name.Equals(descriptor.Name));
            if (cmsModuleConfiguration == null
                && ((constantValues != null && constantValues.Any()) || (appSettingsValues != null && appSettingsValues.Any())
                    || (databaseValues != null && databaseValues.Any())))
            {
                cmsModuleConfiguration = new DynamicModuleElement();
                cmsConfiguration.Modules.Add(cmsModuleConfiguration);
            }
            else
            {
                return;
            }

            foreach (var constantValue in constantValues)
            {
                if (cmsModuleConfiguration.ConfigurationValues.All(d => d.Name != constantValue.Name))
                {
                    cmsModuleConfiguration.ConfigurationValues.Add(constantValue);
                }
            }

            foreach (var settingsValue in appSettingsValues)
            {
                var currentValue = cmsModuleConfiguration.ConfigurationValues.FirstOrDefault(d => d.Name == settingsValue.Key);
                if (currentValue == null)
                {
                    cmsModuleConfiguration.ConfigurationValues.Add(new ConfigurationKeyValueDescriptor
                    {
                        Name = settingsValue.Key,
                        Value = settingsValue.Value,
                        TakenFrom = TakenFrom.WebConfigAppSettings
                    });
                }
                else
                {
                    currentValue.Value = settingsValue.Value;
                    currentValue.TakenFrom = TakenFrom.WebConfigAppSettings;
                }
            }

            foreach (var databaseValue in databaseValues)
            {
                var currentValue = cmsModuleConfiguration.ConfigurationValues.FirstOrDefault(d => d.Name == databaseValue.Name);
                if (currentValue == null)
                {
                    cmsModuleConfiguration.ConfigurationValues.Add(new ConfigurationKeyValueDescriptor
                    {
                        Name = databaseValue.Name,
                        Value = databaseValue.Value,
                        TakenFrom = TakenFrom.Database
                    });
                }
                else
                {
                    currentValue.Value = databaseValue.Value;
                    currentValue.TakenFrom = TakenFrom.Database;
                }
            }
        }

        //public static void Override(this ModuleDescriptor descriptor, ISettingsService settingsService,
        //    ConfigurationKeyValueDescriptor keyValueDescriptor, ICmsConfiguration configuration)
        //{

        //    if (ValueExistsInAppSettings(descriptor.Name, keyValueDescriptor, configuration))
        //    {
        //        return;
        //    }

        //    if (ValueExistsInDatabase(descriptor, settingsService, keyValueDescriptor, configuration))
        //    {
        //        return;
        //    }

        //    UpdateWithDefaultValue(descriptor.Name, keyValueDescriptor, configuration);
        //}

        //private static bool ValueExistsInAppSettings(string moduleName, ConfigurationKeyValueDescriptor keyValueDescriptor, ICmsConfiguration configuration)
        //{
        //    var appSettingsSection = CmsContext.AppSettingsConfiguration;
        //    var appSettingModulePropertyName = string.Format("bcms_{0}_{1}", moduleName, keyValueDescriptor.Key);
        //    var property = appSettingsSection.Settings[appSettingModulePropertyName];

        //    if (property == null)
        //    {
        //        return false;
        //    }

        //    var moduleConfiguration = configuration.Modules.First(m => m.Name.Equals(moduleName));
        //    if (moduleConfiguration != null)
        //    {
        //        moduleConfiguration.SetValue(keyValueDescriptor.Key, property.Value);
        //    }
        //    else
        //    {
        //        configuration.Modules.Add(
        //            new DynamicModuleElement
        //            {
        //                Name = moduleName,
        //                ConfigurationValues = new List<DynamicKeyValue> { new DynamicKeyValue { Key = keyValueDescriptor.Key, Value = property.Value } }
        //            });
        //    }

        //    return true;
        //}

        //private static bool ValueExistsInDatabase(ModuleDescriptor descriptor, ISettingsService settingsService,
        //    ConfigurationKeyValueDescriptor keyValueDescriptor, ICmsConfiguration configuration)
        //{
        //    var moduleSettings = settingsService.GetModuleSettings(descriptor.Id);

        //    if (moduleSettings == null)
        //    {
        //        return false;
        //    }

        //    var property = moduleSettings.First(s => s.Name.Equals(keyValueDescriptor.Key));
        //    if (property == null)
        //    {
        //        return false;
        //    }

        //    var moduleConfiguration = configuration.Modules.First(m => m.Name.Equals(descriptor.Name));
        //    if (moduleConfiguration != null)
        //    {
        //        moduleConfiguration.SetValue(keyValueDescriptor.Key, keyValueDescriptor.Value);
        //    }
        //    else
        //    {
        //        configuration.Modules.Add(
        //            new DynamicModuleElement
        //            {
        //                Name = descriptor.Name,
        //                ConfigurationValues = new List<DynamicKeyValue> { new DynamicKeyValue { Key = keyValueDescriptor.Key, Value = property.Value } }
        //            });
        //    }

        //    return true;
        //}

        //private static void UpdateWithDefaultValue(string moduleName, ConfigurationKeyValueDescriptor keyValueDescriptor, ICmsConfiguration configuration)
        //{
        //    var moduleConfiguration = configuration.Modules.First(m => m.Name.Equals(moduleName));
        //    if (moduleConfiguration != null)
        //    {
        //        moduleConfiguration.SetValue(keyValueDescriptor.Key, keyValueDescriptor.Value);
        //    }
        //    else
        //    {
        //        configuration.Modules.Add(
        //            new DynamicModuleElement
        //            {
        //                Name = moduleName,
        //                ConfigurationValues = new List<DynamicKeyValue> { new DynamicKeyValue { Key = keyValueDescriptor.Key, Value = keyValueDescriptor.Value } }
        //            });
        //    }
        //}
    }
}
