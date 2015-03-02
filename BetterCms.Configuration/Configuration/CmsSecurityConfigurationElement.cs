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
        /// The 'accessControlEnabled' attribute name.
        /// </summary>
        private const string AccessControlEnabledAttribute = "accessControlEnabled";

        /// <summary>
        /// The 'fullAaccessControlListccessRoles' attribute name.
        /// </summary>
        private const string DefaultAccessRulesAttribute = "defaultAccessRules";

        /// <summary>
        /// The 'fullAccessRoles' attribute name.
        /// </summary>
        private const string FullAccessRolesAttribute = "fullAccessRoles";

        /// <summary>
        /// The 'customRoles' attribute name.
        /// </summary>
        private const string CustomRolesAttribute = "customRoles";

        /// <summary>
        /// The 'enableContentEncryption' attribute name.
        /// </summary>
        private const string EnableContentEncryptionAttribute = "encryptionEnabled";

        /// <summary>
        /// The 'contentEncryptionKey' attribute name.
        /// </summary>
        private const string ContentEncryptionKeyAttribute = "encryptionKey";

        /// <summary>
        /// The 'ignoreLocalFileSystemWarning' attribute name.
        /// </summary>
        private const string IgnoreLocalFileSystemWarningAttribute = "ignoreLocalFileSystemWarning";


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
        /// Gets a value indicating whether to a content encryption is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if a content encryption is enabled; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(EnableContentEncryptionAttribute, IsRequired = false, DefaultValue = false)]
        public bool EncryptionEnabled
        {
            get { return Convert.ToBoolean(this[EnableContentEncryptionAttribute]); }
            set { this[EnableContentEncryptionAttribute] = value; }
        }

        /// <summary>
        /// Gets the content encryption key.
        /// </summary>
        /// <value>
        /// The content encryption key.
        /// </value>
        [ConfigurationProperty(ContentEncryptionKeyAttribute, IsRequired = false, DefaultValue = "")]
        public string EncryptionKey
        {
            get { return Convert.ToString(this[ContentEncryptionKeyAttribute]); }
            set { this[ContentEncryptionKeyAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore local file system security warning.
        /// </summary>
        /// <value>
        /// <c>true</c> if ignore local file system warning; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(IgnoreLocalFileSystemWarningAttribute, IsRequired = false, DefaultValue = false)]
        public bool IgnoreLocalFileSystemWarning {
            get
            {
                return (bool)this[IgnoreLocalFileSystemWarningAttribute];
            }
            set
            {
                this[IgnoreLocalFileSystemWarningAttribute] = value;
            } 
        }

        /// <summary>
        /// Gets or sets a value indicating whether access control is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if access control is enabled; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(AccessControlEnabledAttribute, IsRequired = false, DefaultValue = false)]
        public bool AccessControlEnabled
        {
            get { return (bool)this[AccessControlEnabledAttribute]; }
            set { this[AccessControlEnabledAttribute] = value; }
        }

        [ConfigurationProperty(DefaultAccessRulesAttribute, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(AccessControlCollection))]
        public AccessControlCollection DefaultAccessRules
        {
            get
            {
                return this[DefaultAccessRulesAttribute] as AccessControlCollection;
            }
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