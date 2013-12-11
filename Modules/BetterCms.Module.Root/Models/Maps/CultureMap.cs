using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class CultureMap : EntityMapBase<Culture>
    {
        public CultureMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Cultures");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Code).Length(MaxLength.Name).Not.Nullable();
        }
    }
}
