using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class CategoryMap : EntityMapBase<Category>
    {
        public CategoryMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Categories");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
        }
    }
}
