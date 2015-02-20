using BetterModules.Core.Configuration;

namespace BetterCms
{
    public interface ICmsConfigurationLoader : IConfigurationLoader
    {
        ICmsConfiguration LoadCmsConfiguration();
    }
}