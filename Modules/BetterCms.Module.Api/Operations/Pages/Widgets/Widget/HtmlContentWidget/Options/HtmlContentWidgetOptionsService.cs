using BetterModules.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget.Options
{
    public class HtmlContentWidgetOptionsService : Service, IHtmlContentWidgetOptionsService
    {
        private readonly IRepository repository;

        public HtmlContentWidgetOptionsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetHtmlContentWidgetOptionsResponse Get(GetHtmlContentWidgetOptionsRequest request)
        {
            var results = WidgetOptionsHelper.GetWidgetOptionsResponse(repository, request.WidgetId, request);

            return new GetHtmlContentWidgetOptionsResponse { Data = results };
        }
    }
}