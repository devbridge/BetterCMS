using BetterCms.Module.Users.Models;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// User events container.
    /// </summary>
    public partial class UserEvents : EventsBase<UserEvents>
    {
        /// <summary>
        /// Occurs when an role is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Role>> RoleCreated;

        /// <summary>
        /// Occurs when an role is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Role>> RoleUpdated;

        /// <summary>
        /// Occurs when an role is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Role>> RoleDeleted;

        public void OnRoleCreated(Role role)
        {
            if (RoleCreated != null)
            {
                RoleCreated(new SingleItemEventArgs<Role>(role));
            }
        }

        public void OnRoleUpdated(Role role)
        {
            if (RoleUpdated != null)
            {
                RoleUpdated(new SingleItemEventArgs<Role>(role));
            }
        }

        public void OnRoleDeleted(Role role)
        {
            if (RoleDeleted != null)
            {
                RoleDeleted(new SingleItemEventArgs<Role>(role));
            }
        }
    }
}
