// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserEvents.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
