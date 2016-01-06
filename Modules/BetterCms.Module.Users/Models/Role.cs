using System;
using System.Collections.Generic;

using BetterModules.Core.Models;

namespace BetterCms.Module.Users.Models
{
    [Serializable]
    public class Role : EquatableEntity<Role>
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsSystematic { get; set; }

        public virtual IList<UserRole> UserRoles { get; set; }
    }
}