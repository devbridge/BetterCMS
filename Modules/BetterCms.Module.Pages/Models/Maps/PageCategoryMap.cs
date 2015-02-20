using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class PageCategoryMap : EntityMapBase<PageCategory>
    {
        public PageCategoryMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("PageCategories");
            
            References(x => x.Page).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Category).Cascade.SaveUpdate().LazyLoad();
        }
    }
}