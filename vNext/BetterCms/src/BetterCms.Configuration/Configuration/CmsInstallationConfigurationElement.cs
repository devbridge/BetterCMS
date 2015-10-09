using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    /// <summary>
    /// Installation module configuration.
    /// </summary>
    public class CmsInstallationConfigurationElement : ConfigurationElement, ICmsInstallationConfiguration
    {
        private const string Install404ErrorPageAttribute = "Install404ErrorPage";
        private const string Install500ErrorPageAttribute = "Install500ErrorPage";
        private const string InstallDefaultPageAttribute = "InstallDefaultPage";

        [ConfigurationProperty(Install404ErrorPageAttribute, IsRequired = false, DefaultValue = false)]
        public bool Install404ErrorPage
        {
            get { return Convert.ToBoolean(this[Install404ErrorPageAttribute]); }
            set { this[Install404ErrorPageAttribute] = value; }
        }

        [ConfigurationProperty(Install500ErrorPageAttribute, IsRequired = false, DefaultValue = false)]
        public bool Install500ErrorPage
        {
            get { return Convert.ToBoolean(this[Install500ErrorPageAttribute]); }
            set { this[Install500ErrorPageAttribute] = value; }
        }

        [ConfigurationProperty(InstallDefaultPageAttribute, IsRequired = false, DefaultValue = false)]
        public bool InstallDefaultPage
        {
            get { return Convert.ToBoolean(this[InstallDefaultPageAttribute]); }
            set { this[InstallDefaultPageAttribute] = value; }
        }
    }
}
