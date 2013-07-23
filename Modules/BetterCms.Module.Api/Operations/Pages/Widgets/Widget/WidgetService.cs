using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget
{
    public class WidgetService : Service, IWidgetService
    {
        private readonly IHtmlContentWidgetService htmlContentService;
        private readonly IServerControlWidgetService serverControlService;

        public WidgetService(IHtmlContentWidgetService htmlContentService, IServerControlWidgetService serverControlService)
        {
            this.htmlContentService = htmlContentService;
            this.serverControlService = serverControlService;
        }

        IHtmlContentWidgetService IWidgetService.HtmlContent
        {
            get
            {
                return htmlContentService;
            }
        }

        IServerControlWidgetService IWidgetService.ServerControl
        {
            get
            {
                return serverControlService;
            }
        }
    }
}