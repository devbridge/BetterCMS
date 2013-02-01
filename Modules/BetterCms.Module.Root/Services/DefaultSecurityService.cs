using System;
using System.Linq;
using System.Security.Principal;
using System.Web;

using BetterCms.Core.Services;

namespace BetterCms.Module.Root.Services
{
    public class DefaultSecurityService : ISecurityService
    {
        private static readonly char[] RolesSplitter = new[] { ',' };
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSecurityService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DefaultSecurityService(ICmsConfiguration configuration)
        {
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

                if (principal != null)
                {
                    return principal.Identity.Name;
                }

                return "Anonymous";
            }
        }

        /// <summary>
        /// Gets the content management roles.
        /// </summary>
        /// <value>
        /// The content management roles.
        /// </value>
        public string[] ContentManagementRoles
        {
            get { return ParseRoles(configuration.Security.ContentManagementRoles); }
        }

        /// <summary>
        /// Gets the content publishing roles.
        /// </summary>
        /// <value>
        /// The content publishing roles.
        /// </value>
        public string[] ContentPublishingRoles
        {
            get { return ParseRoles(configuration.Security.ContentPublishingRoles); }
        }

        /// <summary>
        /// Gets the page publishing roles.
        /// </summary>
        /// <value>
        /// The page publishing roles.
        /// </value>
        public string[] PagePublishingRoles
        {
            get { return ParseRoles(configuration.Security.PagePublishingRoles); }
        }

        /// <summary>
        /// Gets the publisher roles.
        /// </summary>
        /// <value>
        /// The publisher roles.
        /// </value>
        public string[] PublisherRoles
        {
            get { return ContentPublishingRoles.Union(PagePublishingRoles).ToArray(); }
        }

        /// <summary>
        /// Gets the current principal.
        /// </summary>
        /// <returns></returns>
        public IPrincipal GetCurrentPrincipal()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.User;
            }
            return null;
        }

        /// <summary>
        /// Determines whether specified principal can manage page content.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>
        ///   <c>true</c> if specified principal can manage page content; otherwise, <c>false</c>.
        /// </returns>
        public bool CanManageContent(IPrincipal principal)
        {
            return HasAnyOfRoles(principal, ContentManagementRoles);
        }

        /// <summary>
        /// Determines whether specified principal can publish page content.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>
        ///   <c>true</c> if specified principal can publish page content; otherwise, <c>false</c>.
        /// </returns>
        public bool CanPublishContent(IPrincipal principal)
        {
            return HasAnyOfRoles(principal, ContentPublishingRoles);
        }

        /// <summary>
        /// Determines whether specified principal can publish page.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>
        ///   <c>true</c> if specified principal can publish page; otherwise, <c>false</c>.
        /// </returns>
        public bool CanPublishPage(IPrincipal principal)
        {
            return HasAnyOfRoles(principal, PagePublishingRoles);
        }

        /// <summary>
        /// Determines whether principal has any of specified roles.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="roles">The array of role names.</param>
        /// <returns>
        ///   <c>true</c> if principal has any of specified roles; otherwise, <c>false</c>.
        /// </returns>
        private static bool HasAnyOfRoles(IPrincipal principal, string[] roles)
        {
            var hasAnyOfRoles = false;

            try
            {
                if (principal != null && roles != null && roles.Length > 0)
                {
                    hasAnyOfRoles = principal.Identity.IsAuthenticated && roles.Any(principal.IsInRole);
                }
            }
            catch (Exception e)
            {
            }

            return hasAnyOfRoles;
        }

        /// <summary>
        /// Parses the roles.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>Array of parsed roles</returns>
        private static string[] ParseRoles(string roles)
        {
            if (!string.IsNullOrEmpty(roles))
            {
                return roles.Split(RolesSplitter, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
            }
            return new string[] { };
        }
    }
}