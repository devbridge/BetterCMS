// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICmsUsersConfiguration.cs" company="Devbridge Group LLC">
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
namespace BetterCms
{
    /// <summary>
    /// Users module configuration.
    /// </summary>
    public interface ICmsUsersConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether Better CMS should ask to create a default user on start.
        /// </summary>
        /// <value>
        /// <c>true</c> if Better CMS should ask to create a default user on start; otherwise, <c>false</c>.
        /// </value>
        bool CreateDefaultUserOnStart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Better CMS forms authentication is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if Better CMS forms authentication is enabled; otherwise, <c>false</c>.
        /// </value>
        bool EnableCmsFormsAuthentication { get; set; }
    }
}
