using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class WidgetCategoryMap : EntityMapBase<WidgetCategory>
    {
        public WidgetCategoryMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("WidgetCategories");
            
            References(x => x.Widget).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Category).Cascade.SaveUpdate().LazyLoad();
        }
    }
}