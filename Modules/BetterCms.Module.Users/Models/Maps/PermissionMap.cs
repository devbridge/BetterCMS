using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class PermissionMap : EntityMapBase<Permission>
    {
        public PermissionMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("Permissions");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
        }
    }
}