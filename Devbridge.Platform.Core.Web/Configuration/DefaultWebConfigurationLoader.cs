using System.Web.Configuration;

using Devbridge.Platform.Core.Configuration;

namespace Devbridge.Platform.Core.Web.Configuration
{
    public class DefaultWebConfigurationLoader : DefaultConfigurationLoader
    {
        protected override System.Configuration.Configuration OpenApplicationConfiguration()
        {
            return WebConfigurationManager.OpenWebConfiguration("~/Web.config");
        }
    }
}
