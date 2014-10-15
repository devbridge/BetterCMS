using System.Configuration;

using BetterCms.Configuration;

namespace BetterCms
{
    public interface IConfigurationLoader
    {
        CmsConfigurationSection LoadCmsConfiguration();

        T LoadConfig<T>() where T : ConfigurationSection;
    }
}