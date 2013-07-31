using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget
{
    public interface IWidgetService
    {
        IHtmlContentWidgetService HtmlContent { get; }

        IServerControlWidgetService ServerControl { get; }
    }
}