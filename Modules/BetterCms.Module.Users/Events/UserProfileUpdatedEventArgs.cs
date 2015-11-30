// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfileUpdatedEventArgs.cs" company="Devbridge Group LLC">
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