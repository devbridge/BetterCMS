using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;

using BetterCms.Core.Services;
using BetterCms.Core.Web;

namespace BetterCms.Module.Root.Services
{
    /// <summary>
    /// Default security service contract realization.
    /// </summary>
    public class DefaultSecurityService : ISecurityService
    {
        /// <summary>
        /// The roles splitter.
        /// </summary>
        private static readonly char[] RolesSplitter = new[] { ',' };

        /// <summary>
        /// The configuration service.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// The HTTP context accessor.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSecurityService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public DefaultSecurityService(ICmsConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the name of the get current principal.
        /// </summary>
        /// <value>
        /// The name of the get current principal.
        /// </value>
        public string CurrentPrincipalName
        {
            get
            {
                var principal = GetCurrentPrincipal();

                if (principal != null && principal.Identity.IsAuthenticated)
                {
                    return principal.Identity.Name;
                }

                return "Anonymous";
            }
        }

        /// <summary>
        /// Gets the current principal.
        /// </summary>
        /// <returns>
        /// Current IPrincipal.
        /// </returns>
        public IPrincipal GetCurrentPrincipal()
        {
            var currentHttpContext = httpContextAccessor.GetCurrent();

            if (currentHttpContext == null)
            {
                return Thread.CurrentPrincipal;
            }

            return currentHttpContext.User;
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