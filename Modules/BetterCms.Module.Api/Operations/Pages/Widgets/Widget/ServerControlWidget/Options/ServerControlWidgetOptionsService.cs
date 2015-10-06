using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget.Options
{
    [RoutePrefix("bcms-api")]
    public class ServerControlWidgetOptionsController : ApiController, IServerControlWidgetOptionsService
    {
        private readonly IRepository repository;

        public ServerControlWidgetOptionsController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("widgets/server-control/{WidgetId}/options")]
        public GetServerControlWidgetOptionsResponse Get([ModelBinder(typeof(JsonModelBinder))]GetServerControlWidgetOptionsRequest request)
        {
            var results = WidgetOptionsHelper.GetWidgetOptionsResponse(repository, request.WidgetId, request);

            return new GetServerControlWidgetOptionsResponse { Data = results };
        }
    }
}