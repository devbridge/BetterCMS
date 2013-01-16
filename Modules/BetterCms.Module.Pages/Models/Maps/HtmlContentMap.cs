using BetterCms.Module.Pages.Models.Maps.Predefined;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class HtmlContentMap : HtmlContentSubClassMapBase<HtmlContent>
    {
        public HtmlContentMap()
            : base(PagesModuleDescriptor.ModuleName, "HtmlContents")
        { 
        }
    }
}
