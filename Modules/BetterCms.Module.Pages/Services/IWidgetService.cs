using System.Collections.Generic;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public interface IWidgetService
    {
        void SaveHtmlContentWidget(EditHtmlContentWidgetViewModel model, IList<ContentOptionValuesViewModel> childContentOptionValues, out HtmlContentWidget widget, out HtmlContentWidget originalWidget, bool treatNullsAsLists = true, bool createIfNotExists = false);

        ServerControlWidget SaveServerControlWidget(EditServerControlWidgetViewModel model, bool treatNullsAsLists = true, bool createIfNotExists = false);

        bool DeleteWidget(System.Guid widgetId, int widgetVersion);
    }
}