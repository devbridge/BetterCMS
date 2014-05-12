using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;

namespace BetterCms.Module.Pages.Services
{
    public interface IWidgetService
    {
        void SaveHtmlContentWidget(EditHtmlContentWidgetViewModel model, out HtmlContentWidget widget, out HtmlContentWidget originalWidget);

        ServerControlWidget SaveServerControlWidget(EditServerControlWidgetViewModel model);

        bool DeleteWidget(System.Guid widgetId, int widgetVersion);
    }
}