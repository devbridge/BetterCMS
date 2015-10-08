using System.Security.Principal;

using BetterModules.Core.Security;

namespace BetterCms.Core.Services
{
    /// <summary>
    /// Security service contract.
    /// </summary>
    public interface ISecurityService : IPrincipalProvider
    {
        /// <summary>
        /// Determines whether the specified principal is authorized.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>
        ///   <c>true</c> if the specified principal is authorized; otherwise, <c>false</c>.
        /// </returns>
        bool IsAuthorized(IPrincipal principal, string roles);

        /// <summary>
        /// Determines whether the current principal is authorized.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>
        ///   <c>true</c> if the current principal is authorized; otherwise, <c>false</c>.
        /// </returns>
        bool IsAuthorized(string roles);

        /// <summary>
        /// Determines whether the specified principal has full access role.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns><c>true</c> if user is in full access role, otherwise <c>false</c>.</returns>
        bool HasFullAccess(IPrincipal principal);
    }
}
