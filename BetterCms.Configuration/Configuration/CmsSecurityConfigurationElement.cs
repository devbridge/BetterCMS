using System;
using System.Collections.Generic;
using System.Configuration;

namespace BetterCms.Configuration
{
    /// <summary>
    /// Security configuration element implementation.
    /// </summary>
    public class CmsSecurityConfigurationElement : ConfigurationElement, ICmsSecurityConfiguration
    {
        /// <summary>
        /// The full access roles attribute.
        /// </summary>
        private const string FullAccessRolesAttribute = "fullAccessRoles";

        /// <summary>
        /// The custom roles attribute.
        /// </summary>
        private const string CustomRolesAttribute = "customRoles";

        /// <summary>
        /// Gets or sets the full access roles.
        /// These roles are check despite attribute UseCustomRoles is <c>true</c> or <c>false</c>.
        /// </summary>
        /// <value>
        /// The full access roles.
        /// </value>
        [ConfigurationProperty(FullAccessRolesAttribute, IsRequired = false, DefaultValue = "")]
        public string FullAccessRoles
        {
            get { return Convert.ToString(this[FullAccessRolesAttribute]); }
            set { this[FullAccessRolesAttribute] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether to use custom roles.
        /// </summary>
        /// <value>
        ///   <c>true</c> if custom roles are used; otherwise, <c>false</c>.
        /// </value>
        public bool UseCustomRoles
        {
            get { return CustomRoles.Count > 0; }
        }

        /// <summary>
        /// Gets the custom roles.
        /// </summary>
        /// <value>
        /// The custom roles.
        /// </value>
        [ConfigurationProperty(CustomRolesAttribute, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CustomRolesCollection))]
        public CustomRolesCollection CustomRoles
        {
            get
            {
                return this[CustomRolesAttribute] as CustomRolesCollection;
            }
        }

        /// <summary>
        /// Gets the custom roles.
        /// </summary>
        /// <returns>Permission mapping to roles.</returns>
        public Dictionary<string, string> GetCustomRoles()
        {
            var roles = new Dictionary<string, string>();
            for (var i = 0; i < CustomRoles.Count; i++)
            {
                var role = CustomRoles[i];
                roles.Add(role.Permission, role.Roles);
            }

            return roles;
        }

        /// <summary>
        /// Translates the specified access role.
        /// </summary>
        /// <param name="accessRole">The access role.</param>
        /// <returns>Translated role to custom role.</returns>
        public string Translate(string accessRole)
        {
            var result = CustomRoles.GetElementByKey(accessRole);
            return result != null
                ? result.Roles
                : accessRole;
        }
    }
}