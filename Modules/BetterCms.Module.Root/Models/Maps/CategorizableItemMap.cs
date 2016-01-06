using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class CategorizableItemMap : EntityMapBase<CategorizableItem>
    {
        public CategorizableItemMap() : base(RootModuleDescriptor.ModuleName)
        {
            Table("CategorizableItems");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
        }
    }
}