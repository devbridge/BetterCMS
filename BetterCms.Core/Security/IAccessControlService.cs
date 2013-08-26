using System;
using System.Collections.Generic;
using System.Security.Principal;

using BetterCms.Core.Models;

namespace BetterCms.Core.Security
{
    /// <summary>
    /// Defines interface to get object access level.
    /// </summary>
    public interface IAccessControlService
    {
        /// <summary>
        /// Gets the access level.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        AccessLevel GetAccessLevel<TAccess>(Guid objectId, IPrincipal principal) where TAccess : Entity, IAccess, new();

        /// <summary>
        /// Updates the access control.
        /// </summary>
        /// <param name="userAccessList">The user access list.</param>
        /// <param name="objectId">The object id.</param>
        void UpdateAccessControl<TAccess>(IEnumerable<IAccess> userAccessList, Guid objectId) where TAccess : Entity, IAccess, new();

        /// <summary>
        /// Gets the default access list.
        /// </summary>
        /// <returns></returns>
        List<IAccess> GetDefaultAccessList(IPrincipal principal = null);
    }
}