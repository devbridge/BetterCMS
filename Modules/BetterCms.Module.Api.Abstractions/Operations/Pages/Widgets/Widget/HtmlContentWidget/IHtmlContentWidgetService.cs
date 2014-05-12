using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget.Options;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    public interface IHtmlContentWidgetService
    {
        GetHtmlContentWidgetResponse Get(GetHtmlContentWidgetRequest request);

        PostHtmlContentWidgetResponse Post(PostHtmlContentWidgetRequest request);

        PutHtmlContentWidgetResponse Put(PutHtmlContentWidgetRequest request);

        DeleteHtmlContentWidgetResponse Delete(DeleteHtmlContentWidgetRequest request);

        IHtmlContentWidgetOptionsService Options { get; }
    }
}