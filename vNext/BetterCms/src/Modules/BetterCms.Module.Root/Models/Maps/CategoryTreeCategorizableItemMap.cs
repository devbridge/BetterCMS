using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class CategoryTreeCategorizableItemMap : EntityMapBase<CategoryTreeCategorizableItem>
    {
        public CategoryTreeCategorizableItemMap() : base(RootModuleDescriptor.ModuleName)
        {
            Table("CategoryTreeCategorizableItems");
            
            References(x => x.CategoryTree).Cascade.SaveUpdate().LazyLoad();
            References(x => x.CategorizableItem).Cascade.SaveUpdate().LazyLoad();
        }
    }
}