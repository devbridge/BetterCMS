using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace BetterCms.Configuration
{
    public class DefaultConfigurationLoader : IConfigurationLoader
    {
        private readonly IDictionary<Type, ConfigurationSection> cache = new Dictionary<Type, ConfigurationSection>();

        private static T TraverseConfigSections<T>(ConfigurationSectionGroup group) where T : ConfigurationSection
        {
            foreach (ConfigurationSection section in group.Sections)
            {
                if (Type.GetType(section.SectionInformation.Type, false) == typeof(T))
                {
                    return (T)section;
                }
            }

            foreach (ConfigurationSectionGroup g in group.SectionGroups)
            {
                var section = TraverseConfigSections<T>(g);
                if (section != null)
                {
                    return section;
                }
            }

            return null;
        }

        private static T GetConfig<T>() where T : ConfigurationSection
        {
            T section = null;
            System.Configuration.Configuration config = null;

            try
            {
                config = WebConfigurationManager.OpenWebConfiguration("~/Web.config");
            }
            catch
            {
            }

            if (config == null)
            {
                try
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                catch
                {
                }
            }

            if (config != null && config.RootSectionGroup != null)
            {
                try
                {
                    section = TraverseConfigSections<T>(config.RootSectionGroup);
                }
                catch
                {
                    throw;
                }
            }

            if (section == null)
            {
                // Checking if there are any required properties
                foreach (var member in typeof(T).GetMembers())
                {
                    var configurationPropertyAttribute = Attribute.GetCustomAttribute(member, typeof(ConfigurationPropertyAttribute)) as ConfigurationPropertyAttribute;
                    if ((configurationPropertyAttribute != null) && configurationPropertyAttribute.IsRequired)
                    {
                        throw new ConfigurationErrorsException(string.Format("Required configuration not found: {0}", typeof(T)));
                    }
                }

                return Activator.CreateInstance<T>();
            }

            return section;
        }

        public ICmsConfiguration LoadCmsConfiguration()
        {
            // NOTE: LoadConfig<CmsConfigurationSection> fails to load storage and menu section elements. May be because they are of type ConfigurationElementCollection and not ConfigurationElement
            // return LoadConfig<CmsConfigurationSection>();
            return LoadCmsConfigurationInternal();
        }

        private static ICmsConfiguration LoadCmsConfigurationInternal()
        {
            var config = ConfigurationManager.GetSection("cms");
            return (ICmsConfiguration)config;
        }

        public T LoadConfig<T>() where T : ConfigurationSection
        {
            if (cache.ContainsKey(typeof(T)))
            {
                return (T)cache[typeof(T)];
            }

            var config = GetConfig<T>();
            cache.Add(typeof(T), config);
            return config;
        }
    }
}
