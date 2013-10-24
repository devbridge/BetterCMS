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
        /// <param name="obj">The secured object.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The access level for specific principal</returns>
        AccessLevel GetAccessLevel<TAccessSecuredObject>(TAccessSecuredObject obj, IPrincipal principal) where TAccessSecuredObject : IAccessSecuredObject;

        /// <summary>
        /// Gets the access level according given rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The access level according given rules.</returns>
        AccessLevel GetAccessLevel(IList<IAccessRule> rules, IPrincipal principal);

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