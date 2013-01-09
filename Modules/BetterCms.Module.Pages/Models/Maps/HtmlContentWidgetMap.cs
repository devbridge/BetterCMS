using BetterCms.Core.Models;
using BetterCms.Module.Pages.Models.Maps.Predefined;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class HtmlContentWidgetMap : HtmlContentWidgetSubClassMapBase<HtmlContentWidget>
    {
        public HtmlContentWidgetMap()
            : base(PagesModuleDescriptor.ModuleName, "HtmlContentWidgets")
        {
        }
    }
}
