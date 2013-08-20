using System;
using System.Collections.Generic;
using System.Security.Principal;

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
        AccessLevel GetAccessLevel(Guid objectId, IPrincipal principal);

        /// <summary>
        /// Updates the access control.
        /// </summary>
        /// <param name="userAccessList">The user access list.</param>
        /// <param name="objectId">The object id.</param>
        void UpdateAccessControl(IEnumerable<IUserAccess> userAccessList, Guid objectId);

        /// <summary>
        /// Gets the default access list.
        /// </summary>
        /// <returns></returns>
        List<IUserAccess> GetDefaultAccessList(IPrincipal principal = null);
    }
}