using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class RolePermissionMap : EntityMapBase<RolePermissions>
    {
        public RolePermissionMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("RolePermissions");

            References(f => f.Role).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.Permission).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
        }
    }
}