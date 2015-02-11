using System;

using Devbridge.Platform.Core.Models;

namespace BetterCms.Module.Users.Models
{
    [Serializable]
    public class UserRole : EquatableEntity<UserRole>
    {
        public virtual Role Role { get; set; }

        public virtual User User { get; set; }
    }
}