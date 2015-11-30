// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICmsSecurityConfiguration.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Configuration;

namespace BetterCms
{
    /// <summary>
    /// Security configuration contract.
    /// </summary>
    public interface ICmsSecurityConfiguration
    {
        /// <summary>
        /// Gets a value indicating whether to a content encryption is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if a content encryption is enabled; otherwise, <c>false</c>.
        /// </value>
        bool EncryptionEnabled { get; }

        /// <summary>
        /// Gets the content encryption key.
        /// </summary>
        /// <value>
        /// The content encryption key.
        /// </value>
        string EncryptionKey { get; }


        /// <summary>
        /// Gets or sets a value indicating whether to ignore local file system security warning.
        /// </summary>
        /// <value>
        /// <c>true</c> if ignore local file system warning; otherwise, <c>false</c>.
        /// </value>
        bool IgnoreLocalFileSystemWarning { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether [access control enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [access control enabled]; otherwise, <c>false</c>.
        /// </value>
        bool AccessControlEnabled { get; set; }

        /// <summary>
        /// Gets the default access control list.
        /// </summary>
        /// <value>
        /// The default access control list.
        /// </value>
        AccessControlCollection DefaultAccessRules { get; }
        
        /// <summary>
        /// Gets or sets the full access roles.
        /// </summary>
        /// <value>
        /// The full access roles.
        /// </value>
        string FullAccessRoles { get; set; }

        /// <summary>
        /// Gets a value indicating whether to use custom roles.
        /// </summary>
        /// <value>
        ///   <c>true</c> if custom roles are used; otherwise, <c>false</c>.
        /// </value>
        bool UseCustomRoles { get; }

        /// <summary>
        /// Translates the specified access role.
        /// </summary>
        /// <param name="accessRole">The access role.</param>
        /// <returns>Roles from configuration file.</returns>
        string Translate(string accessRole);

        /// <summary>
        /// Gets the custom roles.
        /// </summary>
        /// <returns>Permission mapping to roles.</returns>
        Dictionary<string, string> GetCustomRoles();
    }
}