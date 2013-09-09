using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    /// <summary>
    /// Installation module configuration.
    /// </summary>
    public class CmsUsersConfigurationElement : ConfigurationElement, ICmsUsersConfiguration
    {
        private const string CreateDefaultUserOnStartAttribute = "createDefaultUserOnStart";
        private const string EnableCmsFormsAuthenticationAttribute = "enableCmsFormsAuthentication";

        [ConfigurationProperty(CreateDefaultUserOnStartAttribute, IsRequired = false, DefaultValue = true)]
        public bool CreateDefaultUserOnStart
        {
            get { return Convert.ToBoolean(this[CreateDefaultUserOnStartAttribute]); }
            set { this[CreateDefaultUserOnStartAttribute] = value; }
        }

        [ConfigurationProperty(EnableCmsFormsAuthenticationAttribute, IsRequired = false, DefaultValue = true)]
        public bool EnableCmsFormsAuthentication
        {
            get { return Convert.ToBoolean(this[EnableCmsFormsAuthenticationAttribute]); }
            set { this[EnableCmsFormsAuthenticationAttribute] = value; }
        }
    }
}
