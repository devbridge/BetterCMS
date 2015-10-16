using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class CategoryTreeMap : EntityMapBase<CategoryTree>
    {
        public CategoryTreeMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("CategoryTrees");

            Map(x => x.Title).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.Macro).Nullable().Length(MaxLength.Text);

            HasMany(f => f.Categories).Table("Categories").KeyColumn("CategoryTreeId").Inverse().Cascade.SaveUpdate().Where("IsDeleted = 0");
            HasMany(x => x.AvailableFor).KeyColumn("CategoryTreeId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
        }
    }
}