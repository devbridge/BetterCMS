// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRoleService.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Users.Services
{
    public interface IRoleService
    {
        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// Created role entity
        /// </returns>
        Models.Role CreateRole(string name, string description = null);

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// Updated role entity
        /// </returns>
        Models.Role UpdateRole(System.Guid id, int version, string name, string description = null);

        /// <summary>
        /// Saves the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="newEntityCreated">if set to <c>true</c> new entity was created.</param>
        /// <param name="createIfNotExists">if set to <c>true</c> create if not exists.</param>
        /// <returns>
        /// Saved role entity
        /// </returns>
        Models.Role SaveRole(System.Guid id, int version, string name, string description, out bool newEntityCreated, bool createIfNotExists = false);

        /// <summary>
        /// Deletes the role by specified role id and version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if role has one or more members and do not delete role.</param>
        /// <returns>
        /// Deleted role entity
        /// </returns>
        Models.Role DeleteRole(System.Guid id, int version, bool throwOnPopulatedRole);

        /// <summary>
        /// Deletes the role by specified role name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if role has one or more members and do not delete role.</param>
        /// <returns></returns>
        Models.Role DeleteRole(string name, bool throwOnPopulatedRole);
    }
}