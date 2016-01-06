using BetterCms.Module.Users.Models;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Role events container.
    /// </summary>
    public partial class UserEvents : EventsBase<UserEvents>
    {
        /// <summary>
        /// Occurs when an user is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<User>> UserCreated;

        /// <summary>
        /// Occurs when an user is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<User>> UserUpdated;

        /// <summary>
        /// Occurs when an user is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<User>> UserDeleted;

        /// <summary>
        /// Occurs when user profile was updated.
        /// </summary>
        public event DefaultEventHandler<UserProfileUpdatedEventArgs> UserProfileUpdated;

        public void OnUserCreated(User user)
        {
            if (UserCreated != null)
            {
                UserCreated(new SingleItemEventArgs<User>(user));
            }
        }

        public void OnUserUpdated(User user)
        {
            if (UserUpdated != null)
            {
                UserUpdated(new SingleItemEventArgs<User>(user));
            }
        }

        public void OnUserDeleted(User user)
        {
            if (UserDeleted != null)
            {
                UserDeleted(new SingleItemEventArgs<User>(user));
            }
        }

        public void OnUserProfileUpdated(User beforeUpdate, User afterUpdate)
        {
            if (UserProfileUpdated != null)
            {
                UserProfileUpdated(new UserProfileUpdatedEventArgs(beforeUpdate, afterUpdate));
            }
        }
    }
}
