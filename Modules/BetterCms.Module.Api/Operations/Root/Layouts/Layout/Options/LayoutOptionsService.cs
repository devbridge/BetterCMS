using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options
{
    [RoutePrefix("bcms-api")]
    public class LayoutOptionsController : ApiController, ILayoutOptionsService
    {
        private readonly IRepository repository;

        public LayoutOptionsController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("layouts/{LayoutId}/options")]
        public GetLayoutOptionsResponse Get([ModelBinder(typeof(JsonModelBinder))]GetLayoutOptionsRequest request)
        {
            var results = LayoutServiceHelper.GetLayoutOptionsResponse(repository, request.LayoutId, request);

            return new GetLayoutOptionsResponse { Data = results };
        }
    }
}