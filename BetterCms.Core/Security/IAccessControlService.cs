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
        /// <param name="obj">The secured object.</param>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        AccessLevel GetAccessLevel<TAccessSecuredObject>(TAccessSecuredObject obj, IPrincipal principal) where TAccessSecuredObject : IAccessSecuredObject;

        /// <summary>
        /// Updates the access control.
        /// </summary>
        /// <param name="obj">The secured object.</param>
        /// <param name="accessRules">The user access list.</param>
        void UpdateAccessControl<TAccessSecuredObject>(TAccessSecuredObject obj, IList<IAccessRule> accessRules) where TAccessSecuredObject : IAccessSecuredObject;

        /// <summary>
        /// Gets the default access list.
        /// </summary>
        /// <returns></returns>
        IList<IAccessRule> GetDefaultAccessList(IPrincipal principal = null);
    }
}