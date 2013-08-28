using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class AccessRuleMap : EntityMapBase<AccessRule>
    {
        public AccessRuleMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("AccessRules");

            Map(x => x.Identity).Column("[Identity]").Length(MaxLength.Name).Not.Nullable();
            Map(x => x.AccessLevel).Not.Nullable();
        }
    }
}
