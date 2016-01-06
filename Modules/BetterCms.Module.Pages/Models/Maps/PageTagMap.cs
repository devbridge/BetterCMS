using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class PageTagMap : EntityMapBase<PageTag>
    {
        public PageTagMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("PageTags");
            
            References(x => x.Page).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Tag).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
