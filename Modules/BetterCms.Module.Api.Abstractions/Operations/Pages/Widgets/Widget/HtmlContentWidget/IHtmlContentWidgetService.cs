using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget.Options;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    public interface IHtmlContentWidgetService
    {
        GetHtmlContentWidgetResponse Get(GetHtmlContentWidgetRequest request);

        IHtmlContentWidgetOptionsService Options { get; }
    }
}