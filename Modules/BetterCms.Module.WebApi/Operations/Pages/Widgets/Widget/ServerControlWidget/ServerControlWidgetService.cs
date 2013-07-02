using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    public class ServerControlWidgetService : Service, IServerControlWidgetService
    {
        public GetServerControlWidgetResponse Get(GetServerControlWidgetRequest request)
        {
            // TODO: need implementation
            return new GetServerControlWidgetResponse
                       {
                           Data = new ServerControlWidgetModel
                                      {
                                          Id = request.WidgetId
                                      }
                       };
        }
    }
}