using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    /// <summary>
    /// API module configuration.
    /// </summary>
    public class CmsApiConfigurationElement : ConfigurationElement, ICmsApiConfiguration
    {
        private const string WebApiDisabledAttribute = "WebApiDisabled";

        [ConfigurationProperty(WebApiDisabledAttribute, IsRequired = false, DefaultValue = false)]
        public bool WebApiDisabled
        {
            get { return Convert.ToBoolean(this[WebApiDisabledAttribute]); }
            set { this[WebApiDisabledAttribute] = value; }
        }
    }
}
