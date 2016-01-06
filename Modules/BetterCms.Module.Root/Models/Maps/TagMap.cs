using BetterCms.Core.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class TagMap : EntityMapBase<Tag>
    {
        public TagMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Tags");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
        }
    }
}
