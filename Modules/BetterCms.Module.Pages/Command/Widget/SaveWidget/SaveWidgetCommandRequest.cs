using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public class SaveWidgetCommandRequest<TWidget>
        where TWidget : WidgetViewModel
    {
        public TWidget Content { get; set; }

        public IList<ContentOptionValuesViewModel> ChildContentOptionValues { get; set; }
    }
}