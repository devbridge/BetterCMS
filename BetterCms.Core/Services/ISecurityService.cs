using System;
using System.Security.Principal;
using System.Web;

namespace BetterCms.Core.Services
{
    public interface ISecurityService
    {
        /// <summary>
        /// Gets the content management roles.
        /// </summary>
        /// <value>
        /// The content management roles.
        /// </value>
        string[] ContentManagementRoles { get; }

        /// <summary>
        /// Gets the content publishing roles.
        /// </summary>
        /// <value>
        /// The content publishing roles.
        /// </value>
        string[] ContentPublishingRoles { get; }

        /// <summary>
        /// Gets the page publishing roles.
        /// </summary>
        /// <value>
        /// The page publishing roles.
        /// </value>
        string[] PagePublishingRoles { get; }

        /// <summary>
        /// Gets the publisher roles.
        /// </summary>
        /// <value>
        /// The publisher roles.
        /// </value>
        string[] PublisherRoles { get; }

        /// <summary>
        /// Gets the current principal.
        /// </summary>
        /// <returns></returns>
        IPrincipal GetCurrentPrincipal();

        /// <summary>
        /// Gets the name of the get current principal.
        /// </summary>
        /// <value>
        /// The name of the get current principal.
        /// </value>
        string CurrentPrincipalName { get; }

        /// <summary>
        /// Determines whether specified principal can manage page content.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if specified principal can manage page content; otherwise, <c>false</c>.
        /// </returns>
        bool CanManageContent(IPrincipal principal);

        /// <summary>
        /// Determines whether specified principal can publish page content.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if specified principal can publish page content; otherwise, <c>false</c>.
        /// </returns>
        bool CanPublishContent(IPrincipal principal);

        /// <summary>
        /// Determines whether specified principal can publish page.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if specified principal can publish page; otherwise, <c>false</c>.
        /// </returns>
        bool CanPublishPage(IPrincipal principal);

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
    }
}
