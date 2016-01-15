// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAccessControlService.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Security.Principal;

using BetterModules.Core.DataContracts;

namespace BetterCms.Core.Security
{
    /// <summary>
    /// Defines interface to get object access level.
    /// </summary>
    public interface IAccessControlService
    {
        /// <summary>
        /// Demands the access level for given secured object by roles and access levels.
        /// </summary>
        /// <typeparam name="TAccessSecuredObject">The type of the access secured object.</typeparam>
        /// <param name="obj">The secured object.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="accessLevel">The access level.</param>
        /// <param name="roles">Demanded roles.</param>
        void DemandAccess<TAccessSecuredObject>(TAccessSecuredObject obj, IPrincipal principal, AccessLevel accessLevel, params string[] roles ) where TAccessSecuredObject : IAccessSecuredObject;

        /// <summary>
        /// Demands the access level by roles only.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="roles">The roles.</param>
        void DemandAccess(IPrincipal principal, params string[] roles);

        /// <summary>
        /// Gets the access level for specific principal.
        /// </summary>
        /// <typeparam name="TAccessSecuredObject">The type of the access secured object.</typeparam>
        /// <param name="obj">The secured object.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="useCache">if set to <c>true</c> use cache.</param>
        /// <returns>
        /// The access level for specific principal
        /// </returns>
        AccessLevel GetAccessLevel<TAccessSecuredObject>(TAccessSecuredObject obj, IPrincipal principal, bool useCache = true) where TAccessSecuredObject : IAccessSecuredObject;

        /// <summary>
        /// Gets the access level according given rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="useCache">if set to <c>true</c> use cache.</param>
        /// <returns>
        /// The access level according given rules.
        /// </returns>
        AccessLevel GetAccessLevel(IList<IAccessRule> rules, IPrincipal principal, bool useCache = true);

        /// <summary>
        /// Updates the access control.
        /// </summary>
        /// <param name="securedObject">The secured object.</param>
        /// <param name="updatedRules">The user access list.</param>
        void UpdateAccessControl<TAccessSecuredObject>(TAccessSecuredObject securedObject, IList<IAccessRule> updatedRules) where TAccessSecuredObject : IAccessSecuredObject;

        /// <summary>
        /// Gets the default access list.
        /// </summary>
        /// <returns></returns>
        IList<IAccessRule> GetDefaultAccessList(IPrincipal principal = null);

        /// <summary>
        /// Gets the list of denied object ids.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="useCache">if set to <c>true</c> use cache.</param>
        /// <returns>
        /// Enumerable list of denied object ids
        /// </returns>
        IEnumerable<Guid> GetDeniedObjects<TEntity>(bool useCache = true) 
            where TEntity : IEntity, IAccessSecuredObject;

        /// <summary>
        /// Gets the principal denied object.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="useCache">if set to <c>true</c> use cache.</param>
        /// <returns></returns>
        IEnumerable<Guid> GetPrincipalDeniedObjects<TEntity>(IPrincipal principal, bool useCache = true)
            where TEntity : IEntity, IAccessSecuredObject;
    }
}