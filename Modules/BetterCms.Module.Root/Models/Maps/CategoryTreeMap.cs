using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class CategoryTreeMap : EntityMapBase<CategoryTree>
    {
        public CategoryTreeMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("CategoryTrees");

            Map(x => x.Title).Not.Nullable().Length(MaxLength.Name);
            HasMany(f => f.Categories).Table("Categories").KeyColumn("CategoryTreeId").Inverse().Cascade.SaveUpdate().Where("IsDeleted = 0");
        }
    }
}