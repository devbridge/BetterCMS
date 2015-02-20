using BetterModules.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class UserRoleMap : EntityMapBase<UserRole>
    {
        public UserRoleMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("UserRoles");

            References(f => f.Role).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.User).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
        }
    }
}