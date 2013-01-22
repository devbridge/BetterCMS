using System;
using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models
{
    [Serializable]
    public class RolePremissions : EquatableEntity<RolePremissions>
    {
        public virtual Role Role { get; set; }

        public virtual Premission Premission { get; set; }
        
    }
}