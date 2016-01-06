using System.Configuration;
using System.Web.Configuration;

using BetterModules.Core.Configuration;

namespace BetterCms.Configuration
{
    public class CmsConfigurationLoader : DefaultConfigurationLoader, ICmsConfigurationLoader
    {
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

        protected override System.Configuration.Configuration OpenApplicationConfiguration()
        {
            return WebConfigurationManager.OpenWebConfiguration("~/Web.config");
        }
    }
}
