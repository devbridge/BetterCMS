using System;

using BetterCms.Module.Users.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class UserProfileUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileUpdatedEventArgs" /> class.
        /// </summary>
        /// <param name="beforeUpdate">The before update.</param>
        /// <param name="afterUpdate">The after update.</param>
        public UserProfileUpdatedEventArgs(User beforeUpdate, User afterUpdate)
        {
            BeforeUpdate = beforeUpdate;
            AfterUpdate = afterUpdate;
        }

        /// <summary>
        /// Gets or sets the before update.
        /// </summary>
        /// <value>
        /// The before update.
        /// </value>
        public User BeforeUpdate { get; set; }

        /// <summary>
        /// Gets or sets the user entity after update.
        /// </summary>
        /// <value>
        /// The user entity after update.
        /// </value>
        public User AfterUpdate { get; set; }
    }
}