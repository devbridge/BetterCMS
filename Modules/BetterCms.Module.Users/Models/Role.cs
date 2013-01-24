using System;
using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models
{
    [Serializable]
    public class Role : EquatableEntity<Role>
    {
        public virtual string Name { get; set; }
    }
}