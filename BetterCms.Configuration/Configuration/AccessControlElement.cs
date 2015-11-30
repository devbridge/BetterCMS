// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControlElement.cs" company="Devbridge Group LLC">
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
    public class AccessControlElement : ConfigurationElement
    {
        private const string IdentityAttribute = "identity";

        private const string AccessLevelAttribute = "accessLevel";

        private const string IsRoleAttribute = "isRole";

        /// <summary>
        /// Gets or sets the role or user.
        /// </summary>
        /// <value>
        /// The role or user.
        /// </value>
        [ConfigurationProperty(IdentityAttribute, IsRequired = true)]
        public string Identity
        {
            get { return Convert.ToString(this[IdentityAttribute]); }
            set { this[IdentityAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the access level.
        /// </summary>
        /// <value>
        /// The access level.
        /// </value>
        [ConfigurationProperty(AccessLevelAttribute, IsRequired = true)]
        public string AccessLevel
        {
            get { return Convert.ToString(this[AccessLevelAttribute]); }
            set { this[AccessLevelAttribute] = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this access rule is role based.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this access rule is role bases; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(IsRoleAttribute, IsRequired = false, DefaultValue = true)]
        public bool IsRole
        {
            get { return Convert.ToBoolean(this[IsRoleAttribute]); }
            set { this[IsRoleAttribute] = value; }
        }

    }
}