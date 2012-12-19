using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class User : EquatableEntity<User>
    {
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string DisplayName { get; set; }
    }
}