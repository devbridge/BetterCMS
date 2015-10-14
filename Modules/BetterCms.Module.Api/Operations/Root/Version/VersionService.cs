using System.Web.Http;

namespace BetterCms.Module.Api.Operations.Root.Version
{
    [RoutePrefix("bcms-api")]
    public class VersionController : ApiController, IVersionService
    {
        private readonly ICmsConfiguration configuration;

        public VersionController(ICmsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [Route("current-version")]
        public GetVersionResponse Get([FromUri]GetVersionRequest request = null)
        {
            return new GetVersionResponse
                       {
                           Data = new VersionModel
                                      {
                                          Version = configuration.Version
                                      }
                       };
        }
    }
}