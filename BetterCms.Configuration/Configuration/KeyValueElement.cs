using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class KeyValueElement : ConfigurationElement
    {
        private const string KeyAttribute = "key";
        private const string ValueAttribute = "value";

        /// <summary>
        /// Gets or sets the link text of the the link that is being added to the sidemenu.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [ConfigurationProperty(KeyAttribute, IsRequired = true)]
        public string Key
        {
            get { return Convert.ToString(this[KeyAttribute]); }
            set { this[KeyAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [ConfigurationProperty(ValueAttribute, IsRequired = true)]
        public string Value
        {
            get { return Convert.ToString(this[ValueAttribute]); }
            set { this[ValueAttribute] = value; }
        }
    }
}