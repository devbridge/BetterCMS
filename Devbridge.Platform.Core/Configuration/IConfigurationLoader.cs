using System.Configuration;

namespace Devbridge.Platform.Core.Configuration
{
    public interface IConfigurationLoader
    {
        T LoadConfig<T>() where T : ConfigurationSection;
    }
}