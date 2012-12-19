using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class LinkElement : ConfigurationElement
    {
        private const string NameAttribute = "name";
        private const string UrlAttribute = "url";

        /// <summary>
        /// Gets or sets the link text of the the link that is being added to the sidemenu.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [ConfigurationProperty(NameAttribute, IsRequired = true)]
        public string Name
        {
            get { return Convert.ToString(this[NameAttribute]); }
            set { this[NameAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [ConfigurationProperty(UrlAttribute, IsRequired = true)]
        public string Url
        {
            get { return Convert.ToString(this[UrlAttribute]); }
            set { this[UrlAttribute] = value; }
        }
    }
}