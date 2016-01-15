// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRole.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Core.Security
{
    /// <summary>
    /// Implements permission interface properties.
    /// </summary>
    public class UserRole : IUserRole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRole" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="localizedName">Name of the localized.</param>
        public UserRole(string name, Func<string> localizedName)
        {
            Name = name;
            LocalizedName = localizedName;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the localized name.
        /// </summary>
        /// <value>
        /// The localized name.
        /// </value>
        public Func<string> LocalizedName { get; private set; }
    }
}
