using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class CmsSecurityConfigurationElement : ConfigurationElement, ICmsSecurityConfiguration
    {
        /// <summary>
        /// The full access roles attribute.
        /// </summary>
        private const string FullAccessRolesAttribute = "fullAccessRoles";

        /// <summary>
        /// The use custom roles attribute.
        /// </summary>
        private const string UseCustomRolesAttribute = "useCustomRoles";

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
        /// Gets or sets a value indicating whether to use custom roles.
        /// </summary>
        /// <value>
        ///   <c>true</c> if custom roles are used otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(UseCustomRolesAttribute, IsRequired = false, DefaultValue = false)]
        public bool UseCustomRoles
        {
            get { return Convert.ToBoolean(this[UseCustomRolesAttribute]); }
            set { this[UseCustomRolesAttribute] = value; }
        }

        /// <summary>
        /// Translates the specified access role.
        /// </summary>
        /// <param name="accessRole">The access role.</param>
        /// <returns>Translated role to custom role.</returns>
        public string Translate(string accessRole)
        {
            var result = Convert.ToString(this[accessRole]);
            return string.IsNullOrEmpty(result)
                ? accessRole
                : result;
        }



        // TODO: remove below - obsolete.

        private const string ContentManagementRolesAttribute = "contentManagementRoles";
        private const string ContentPublishingRolesAttribute = "contentPublishingRoles";
        private const string PagePublishingRolesAttribute = "pagePublishingRoles";

        [ConfigurationProperty(ContentManagementRolesAttribute, IsRequired = false)]
        public string ContentManagementRoles
        {
            get { return Convert.ToString(this[ContentManagementRolesAttribute]); }
            set { this[ContentManagementRolesAttribute] = value; }
        } 

        [ConfigurationProperty(ContentPublishingRolesAttribute, IsRequired = false)]
        public string ContentPublishingRoles
        {
            get { return Convert.ToString(this[ContentPublishingRolesAttribute]); }
            set { this[ContentPublishingRolesAttribute] = value; }
        }      

        [ConfigurationProperty(PagePublishingRolesAttribute, IsRequired = false)]
        public string PagePublishingRoles
        {
            get { return Convert.ToString(this[PagePublishingRolesAttribute]); }
            set { this[PagePublishingRolesAttribute] = value; }
        }
    }
}