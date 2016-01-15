// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessRule.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Security;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    /// <summary>
    /// PageAccess entity.
    /// </summary>
    [Serializable]
    public class AccessRule : EquatableEntity<AccessRule>, IAccessRule, IDeleteableEntity
    {
        /// <summary>
        /// Gets or sets the user/role.
        /// </summary>
        /// <value>
        /// The user/role.
        /// </value>
        public virtual string Identity { get; set; }

        /// <summary>
        /// Gets or sets the access level.
        /// </summary>
        /// <value>
        /// The access level.
        /// </value>
        public virtual AccessLevel AccessLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this rule applies for role.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance applies for role; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsForRole { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Identity: {1}, AccessLevel: {2}", base.ToString(), Identity, AccessLevel);
        }
    }
}