using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class CmsSecurityConfigurationElement : ConfigurationElement, ICmsSecurityConfiguration
    {
        private const string FullAccessRolesAttribute = "fullAccessRoles";

        [ConfigurationProperty(FullAccessRolesAttribute, IsRequired = false)]
        public string FullAccessRoles
        {
            get { return Convert.ToString(this[FullAccessRolesAttribute]); }
            set { this[FullAccessRolesAttribute] = value; }
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