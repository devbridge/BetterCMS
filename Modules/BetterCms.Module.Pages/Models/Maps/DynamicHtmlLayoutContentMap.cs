using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class DynamicHtmlLayoutContentMap : EntitySubClassMapBase<DynamicHtmlLayoutContent>
    {
        public DynamicHtmlLayoutContentMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("DynamicHtmlLayoutContents");

            Map(x => x.Html).Not.Nullable();   
        }
    }
}
