using BetterCms.Core.Models;
using BetterCms.Core.Security;

namespace BetterCms.Module.Root.Models.Maps
{
    public class AccessRuleMap : EntityMapBase<AccessRule>
    {
        public AccessRuleMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("AccessRules");

            Map(x => x.Identity).Column("[Identity]").Length(MaxLength.Max).Not.Nullable().CustomType<EncryptableString>();
            Map(x => x.AccessLevel).Not.Nullable();
            Map(x => x.IsForRole).Not.Nullable();
        }
    }
}
