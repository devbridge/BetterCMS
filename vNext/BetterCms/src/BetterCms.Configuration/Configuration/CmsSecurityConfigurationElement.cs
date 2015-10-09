using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterCms.Configuration
{
    /// <summary>
    /// Security configuration element implementation.
    /// </summary>
    public class CmsSecurityConfigurationElement //: ConfigurationElement, ICmsSecurityConfiguration
    {
        public CmsSecurityConfigurationElement()
        {
            DefaultAccessRules = new List<AccessControlElement>();
            CustomRoles = new List<CustomRoleElement>();
        }

        /// <summary>
        /// Gets or sets the full access roles.
        /// These roles are check despite attribute UseCustomRoles is <c>true</c> or <c>false</c>.
        /// </summary>
        /// <value>
        /// The full access roles.
        /// </value>
        public string FullAccessRoles { get; set; } = "";

        /// <summary>
        /// Gets a value indicating whether to use custom roles.
        /// </summary>
        /// <value>
        ///   <c>true</c> if custom roles are used; otherwise, <c>false</c>.
        /// </value>
        public bool UseCustomRoles => CustomRoles.Count > 0;

        /// <summary>
        /// Gets a value indicating whether to a content encryption is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if a content encryption is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EncryptionEnabled { get; set; } = false;

        /// <summary>
        /// Gets the content encryption key.
        /// </summary>
        /// <value>
        /// The content encryption key.
        /// </value>
        public string EncryptionKey { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether to ignore local file system security warning.
        /// </summary>
        /// <value>
        /// <c>true</c> if ignore local file system warning; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreLocalFileSystemWarning { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether access control is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if access control is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool AccessControlEnabled { get; set; } = false;
        
        public IList<AccessControlElement> DefaultAccessRules { get; set; }

        /// <summary>
        /// Gets the custom roles.
        /// </summary>
        /// <value>
        /// The custom roles.
        /// </value>
        public IList<CustomRoleElement> CustomRoles { get; set; }

        /// <summary>
        /// Gets the custom roles.
        /// </summary>
        /// <returns>Permission mapping to roles.</returns>
        public Dictionary<string, string> GetCustomRoles()
        {
            return CustomRoles.ToDictionary(role => role.Permission, role => role.Roles);
        }

        /// <summary>
        /// Translates the specified access role.
        /// </summary>
        /// <param name="accessRole">The access role.</param>
        /// <returns>Translated role to custom role.</returns>
        public string Translate(string accessRole)
        {
            var result = CustomRoles.FirstOrDefault(x => x.Permission == accessRole);
            return result != null
                ? result.Roles
                : accessRole;
        }
    }
}