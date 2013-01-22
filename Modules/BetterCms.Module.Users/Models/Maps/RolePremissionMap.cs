using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class RolePremissionMap : EntityMapBase<RolePremissions>
    {
        public RolePremissionMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("RolePremissions");

            References(f => f.Role).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.Premission).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
        }
    }
}