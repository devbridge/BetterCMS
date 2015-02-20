using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class CustomOptionMap : EntityMapBase<CustomOption>
    {
        public CustomOptionMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("CustomOptions");

            Map(x => x.Title).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Identifier).Length(MaxLength.Name).Not.Nullable();
        }
    }
}