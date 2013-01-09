using BetterCms.Core.Models;
using BetterCms.Module.Pages.Models.Maps.Predefined;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class HtmlContentWidgetHistoryMap : HtmlContentWidgetSubClassMapBase<HtmlContentWidgetHistory>
    {
        public HtmlContentWidgetHistoryMap()
            : base(PagesModuleDescriptor.ModuleName, "HtmlContentWidgetHistory")
        {
        }
    }
}
