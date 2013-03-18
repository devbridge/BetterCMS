using System.Collections.Generic;

namespace BetterCms
{
    /// <summary>
    /// Security configuration contract.
    /// </summary>
    public interface ICmsSecurityConfiguration
    {
        /// <summary>
        /// Gets or sets the full access roles.
        /// </summary>
        /// <value>
        /// The full access roles.
        /// </value>
        string FullAccessRoles { get; set; }

        /// <summary>
        /// Gets a value indicating whether to use custom roles.
        /// </summary>
        /// <value>
        ///   <c>true</c> if custom roles are used; otherwise, <c>false</c>.
        /// </value>
        bool UseCustomRoles { get; }

        /// <summary>
        /// Translates the specified access role.
        /// </summary>
        /// <param name="accessRole">The access role.</param>
        /// <returns>Roles from configuration file.</returns>
        string Translate(string accessRole);

        /// <summary>
        /// Gets the custom roles.
        /// </summary>
        /// <returns>Permission mapping to roles.</returns>
        Dictionary<string, string> GetCustomRoles();
    }
}