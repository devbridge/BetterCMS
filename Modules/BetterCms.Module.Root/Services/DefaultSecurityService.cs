using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.Services;

using BetterModules.Core.Web.Security;
using BetterModules.Core.Web.Web;

namespace BetterCms.Module.Root.Services
{
    /// <summary>
    /// Default security service contract realization.
    /// </summary>
    public class DefaultSecurityService : DefaultWebPrincipalProvider, ISecurityService
    {
        /// <summary>
        /// The roles splitter.
        /// </summary>
        private static readonly char[] RolesSplitter = { ',' };

        /// <summary>
        /// The configuration service.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSecurityService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public DefaultSecurityService(ICmsConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Determines whether the specified principal is authorized.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>
        ///   <c>true</c> if the specified principal is authorized; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAuthorized(IPrincipal principal, string roles)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(roles))
                {
                    return true;
                }

                if (HasFullAccess(principal))
                {
                    // User is in full access role.
                    return true;
                }

                var roleList = ParseRoles(roles);

                // Check for configuration defined user roles.
                bool useCustomRoles = configuration.Security.UseCustomRoles;
                foreach (var role in roleList)
                {
                    if (principal.IsInRole(role))
                    {
                        return true;
                    }

                    if (useCustomRoles)
                    {
                        var translatedRoles = ParseRoles(configuration.Security.Translate(role));
                        if (translatedRoles.Any(principal.IsInRole))
                        {
                            return true;
                        }                        
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the current principal is authorized.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>
        ///   <c>true</c> if the current principal is authorized; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAuthorized(string roles)
        {
            return IsAuthorized(GetCurrentPrincipal(), roles);
        }

        /// <summary>
        /// Determines whether the specified principal has full access role.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>
        ///   <c>true</c> if user is in full access role, otherwise <c>false</c>.
        /// </returns>
        public bool HasFullAccess(IPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var fullAccessRoles = ParseRoles(configuration.Security.FullAccessRoles);
                return fullAccessRoles.Any(principal.IsInRole);
            }

            return false;
        }

        /// <summary>
        /// Parses the roles.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>Array of parsed roles</returns>
        private static IEnumerable<string> ParseRoles(string roles)
        {
            return !string.IsNullOrEmpty(roles)
                ? roles.Split(RolesSplitter, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(role => role.Trim()).ToArray()
                : new string[] { };
        }
    }
}