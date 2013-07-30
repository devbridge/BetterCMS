using System;

using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.Users.Models
{
    [Serializable]
    public class Users : EquatableEntity<Users>
    {
        public virtual string UserName { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual string Salt { get; set; }

        public virtual MediaImage Image { get; set; }

    }
}