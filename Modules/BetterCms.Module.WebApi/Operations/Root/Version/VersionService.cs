using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Version
{
    public class VersionService : Service
    {
        private readonly ICmsConfiguration configuration;

        public VersionService(ICmsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public GetVersionResponse Get(GetVersionRequest request)
        {            
            return new GetVersionResponse {
                                           Status = "ok",
                                           Data = configuration.Version
                                       };
        }
    }
}