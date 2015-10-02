using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [RoutePrefix("bcms-api")]
    public class LayoutRegionsController : ApiController, ILayoutRegionsService
    {
        private readonly IRepository repository;

        public LayoutRegionsController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("layouts/{LayoutId}/regions")]
        public GetLayoutRegionsResponse Get([ModelBinder(typeof(JsonModelBinder))]GetLayoutRegionsRequest request)
        {
            var listResponse = LayoutServiceHelper.GetLayoutRegionsResponse(repository, request.LayoutId, request);

            return new GetLayoutRegionsResponse { Data = listResponse };
        }
    }
}