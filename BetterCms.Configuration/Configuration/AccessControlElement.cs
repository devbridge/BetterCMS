using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class AccessControlElement : ConfigurationElement
    {
        private const string IdentityAttribute = "identity";

        private const string AccessLevelAttribute = "accessLevel";

        /// <summary>
        /// Gets or sets the role or user.
        /// </summary>
        /// <value>
        /// The role or user.
        /// </value>
        [ConfigurationProperty(IdentityAttribute, IsRequired = true)]
        public string Identity
        {
            get { return Convert.ToString(this[IdentityAttribute]); }
            set { this[IdentityAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the access level.
        /// </summary>
        /// <value>
        /// The access level.
        /// </value>
        [ConfigurationProperty(AccessLevelAttribute, IsRequired = true)]
        public string AccessLevel
        {
            get { return Convert.ToString(this[AccessLevelAttribute]); }
            set { this[AccessLevelAttribute] = value; }
        }

    }
}