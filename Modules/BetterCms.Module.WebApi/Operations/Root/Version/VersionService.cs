using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Version
{
    public class VersionService : Service, IVersionService
    {
        private readonly ICmsConfiguration configuration;

        public VersionService(ICmsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public GetVersionResponse Get(GetVersionRequest request = null)
        {            
            return new GetVersionResponse {
                                           Data = configuration.Version
                                       };
        }
    }
}