using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models
{
    public class UserRoles : EquatableEntity<RolePermissions>
    {
        public virtual Role Role { get; set; }

        public virtual Users User { get; set; }

    }
}