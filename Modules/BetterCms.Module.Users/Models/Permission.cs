using System;
using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models
{
    [Serializable]
    public class Permission : EquatableEntity<Permission>
    {
        public virtual string Name { get; set; }
    }
}