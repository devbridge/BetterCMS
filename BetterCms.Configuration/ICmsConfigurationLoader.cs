using Devbridge.Platform.Core.Configuration;

namespace BetterCms
{
    public interface ICmsConfigurationLoader : IConfigurationLoader
    {
        ICmsConfiguration LoadCmsConfiguration();
    }
}