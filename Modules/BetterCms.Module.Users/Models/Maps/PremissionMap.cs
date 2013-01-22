using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class PremissionMap : EntityMapBase<Premission>
    {
        public PremissionMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("Premissions");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
        }
    }
}