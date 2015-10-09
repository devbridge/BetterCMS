using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    /// <summary>
    /// Configuration custom role element for security.
    /// </summary>
    public class CustomRoleElement : ConfigurationElement
    {
        /// <summary>
        /// The permission attribute.
        /// </summary>
        private const string PermissionAttribute = "permission";

        /// <summary>
        /// The roles attribute.
        /// </summary>
        private const string RolesAttribute = "roles";

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>
        /// The permission.
        /// </value>
        [ConfigurationProperty(PermissionAttribute, IsRequired = true)]
        public string Permission
        {
            get { return Convert.ToString(this[PermissionAttribute]); }
            set { this[PermissionAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        [ConfigurationProperty(RolesAttribute, IsRequired = true)]
        public string Roles
        {
            get { return Convert.ToString(this[RolesAttribute]); }
            set { this[RolesAttribute] = value; }
        }
    }
}