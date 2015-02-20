using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.Models;

using BetterModules.Core.Models;

namespace BetterCms.Module.Users.Models
{
    [Serializable]
    public class User : EquatableEntity<User>
    {
        public virtual string UserName { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual string Salt { get; set; }

        public virtual MediaImage Image { get; set; }

        public virtual IList<UserRole> UserRoles { get; set; }

        public virtual User CopyDataTo(User user)
        {
            user.UserName = UserName;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = Email;
            user.Password = Password;
            user.Salt = Salt;
            user.Image = Image;

            foreach (var userRole in UserRoles)
            {
                if (user.UserRoles == null)
                {
                    user.UserRoles = new List<UserRole>();
                }

                user.UserRoles.Add(new UserRole { User = user, Role = userRole.Role });
            }

            return user;
        }
    }
}