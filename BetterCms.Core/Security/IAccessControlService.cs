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
        /// <param name="obj">The secured object.</param>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        AccessLevel GetAccessLevel<TAccessSecuredObject>(TAccessSecuredObject obj, IPrincipal principal) where TAccessSecuredObject : IAccessSecuredObject;

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
    }
}