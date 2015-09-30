using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget.Options
{
    [RoutePrefix("bcms-api")]
    public class HtmlContentWidgetOptionsController : ApiController, IHtmlContentWidgetOptionsService
    {
        private readonly IRepository repository;

        public HtmlContentWidgetOptionsController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("widgets/html-content/{WidgetId}/options")]
        public GetHtmlContentWidgetOptionsResponse Get([ModelBinder(typeof(JsonModelBinder))]GetHtmlContentWidgetOptionsRequest request)
        {
            var results = WidgetOptionsHelper.GetWidgetOptionsResponse(repository, request.WidgetId, request);

            return new GetHtmlContentWidgetOptionsResponse { Data = results };
        }
    }
}