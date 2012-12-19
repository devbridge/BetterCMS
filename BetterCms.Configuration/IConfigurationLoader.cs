using System.Configuration;

namespace BetterCms
{
    public interface IConfigurationLoader
    {
        ICmsConfiguration LoadCmsConfiguration();

        T LoadConfig<T>() where T : ConfigurationSection;
    }
}