using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.Modules.Registration;
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
        /// The modules registration.
        /// </summary>
        private readonly IModulesRegistration modulesRegistration;

        /// <summary>
        /// The HTTP context accessor.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSecurityService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="modulesRegistration">The modules registration.</param>
        public DefaultSecurityService(ICmsConfiguration configuration, IHttpContextAccessor httpContextAccessor, IModulesRegistration modulesRegistration)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.modulesRegistration = modulesRegistration;
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

                if (principal != null)
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

            return currentHttpContext != null
                ? currentHttpContext.User
                : null;
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>Role list.</returns>
        public string[] GetAllRoles()
        {
            return modulesRegistration.GetUserAccessRoles().Select(m => m.Name).ToArray();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns>Better CMS security configuration.</returns>
        public ICmsSecurityConfiguration GetConfiguration()
        {
            return configuration.Security;
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

                var fullAccessRoles = ParseRoles(configuration.Security.FullAccessRoles);
                if (fullAccessRoles.Any(principal.IsInRole))
                {
                    // User is in full access role.
                    return true;
                }

                var roleList = ParseRoles(roles);

                if (!configuration.Security.UseCustomRoles)
                {
                    return roleList.Any(principal.IsInRole);
                }

                // Check for configuration defined user roles.
                foreach (var role in roleList)
                {
                    var translatedRoles = ParseRoles(configuration.Security.Translate(role));
                    if (translatedRoles.Any(principal.IsInRole))
                    {
                        return true;
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
        /// Parses the roles.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>Array of parsed roles</returns>
        private static IEnumerable<string> ParseRoles(string roles)
        {
            return !string.IsNullOrEmpty(roles)
                ? roles.Split(RolesSplitter, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray()
                : new string[] { };
        }
    }
}