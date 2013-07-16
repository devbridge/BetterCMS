using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Api.Operations.Root.GetTagByName;
using BetterCms.Module.Api.Operations.Root.GetTags;
using BetterCms.Module.Api.Operations.Root.GetVersion;

using Microsoft.Web.Mvc;


namespace BetterCms.Module.Api.Controllers
{
    /// <summary>
    /// Blogs API controller.
    /// </summary>
    [ActionLinkArea(ApiModuleDescriptor.WebApiAreaName)]
    public class RootController : CmsApiControllerBase, IRootOperationsContext
    {
        private readonly IRootOperationsContext rootOperations;

        public RootController(IRootOperationsContext rootOperations)
        {
            this.rootOperations = rootOperations;
        }

        public GetVersionResponse GetVersion()
        {
            return rootOperations.GetVersion();
        }

        public GetTagsResponse GetTags(GetTagsRequest request)
        {
            return rootOperations.GetTags(request);
        }

        public GetTagByNameResponse GetTagByName(GetTagByNameRequest request)
        {
            return rootOperations.GetTagByName(request);
        }
    }
}
