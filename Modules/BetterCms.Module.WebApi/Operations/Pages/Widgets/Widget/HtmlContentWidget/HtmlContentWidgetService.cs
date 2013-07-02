using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    public class HtmlContentWidgetService : Service, IHtmlContentWidgetService
    {
        public GetHtmlContentWidgetResponse Get(GetHtmlContentWidgetRequest request)
        {
            // TODO: need implementation
            return new GetHtmlContentWidgetResponse
                       {
                           Data = new HtmlContentWidgetModel
                                      {
                                          Id = request.WidgetId
                                      }
                       };
        }
    }
}