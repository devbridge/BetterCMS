using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class CategoryMap : EntityMapBase<Category>
    {
        public CategoryMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Categories");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Macro).Nullable().Length(MaxLength.Text);
            Map(x => x.DisplayOrder).Not.Nullable();

            References(x => x.CategoryTree).Cascade.SaveUpdate().LazyLoad();
            References(f => f.ParentCategory).Cascade.SaveUpdate().Nullable().LazyLoad();
            HasMany(f => f.ChildCategories).Table("Categories").KeyColumn("ParentCategoryId").Inverse().Cascade.SaveUpdate().Where("IsDeleted = 0").LazyLoad();
        }
    }
}
