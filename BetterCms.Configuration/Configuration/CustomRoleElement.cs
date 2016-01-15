// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomRoleElement.cs" company="Devbridge Group LLC">
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
using System.Configuration;

namespace BetterCms.Configuration
{
    /// <summary>
    /// Configuration custom role element for security.
    /// </summary>
    public class CustomRoleElement : ConfigurationElement
    {
        /// <summary>
        /// The permission attribute.
        /// </summary>
        private const string PermissionAttribute = "permission";

        /// <summary>
        /// The roles attribute.
        /// </summary>
        private const string RolesAttribute = "roles";

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>
        /// The permission.
        /// </value>
        [ConfigurationProperty(PermissionAttribute, IsRequired = true)]
        public string Permission
        {
            get { return Convert.ToString(this[PermissionAttribute]); }
            set { this[PermissionAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        [ConfigurationProperty(RolesAttribute, IsRequired = true)]
        public string Roles
        {
            get { return Convert.ToString(this[RolesAttribute]); }
            set { this[RolesAttribute] = value; }
        }
    }
}