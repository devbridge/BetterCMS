using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;

namespace BetterCms.Module.Pages.Services
{
    public interface IWidgetService
    {
        void SaveHtmlContentWidget(EditHtmlContentWidgetViewModel model, out HtmlContentWidget widget, out HtmlContentWidget originalWidget, bool treatNullsAsLists = true, bool createIfNotExists = false);

        ServerControlWidget SaveServerControlWidget(EditServerControlWidgetViewModel model, bool treatNullsAsLists = true, bool createIfNotExists = false);

        bool DeleteWidget(System.Guid widgetId, int widgetVersion);
    }
}