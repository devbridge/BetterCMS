using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class UserRolesMap : EntityMapBase<UserRoles>
    {
        public UserRolesMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("UserRoles");

            References(f => f.Role).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.User).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
        }
    }
}