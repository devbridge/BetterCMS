using BetterCms.Module.Pages.Models.Maps.Predefined;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class HtmlContentHistoryMap : HtmlContentSubClassMapBase<HtmlContentHistory>
    {
        public HtmlContentHistoryMap()
            : base(PagesModuleDescriptor.ModuleName, "HtmlContentHistory")
        { 
        }
    }
}
