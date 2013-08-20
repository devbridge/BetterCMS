using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class AccessControlElement : ConfigurationElement
    {
        private const string RoleOrUserAttribute = "roleOrUser";

        private const string AccessLevelAttribute = "accessLevel";

        /// <summary>
        /// Gets or sets the role or user.
        /// </summary>
        /// <value>
        /// The role or user.
        /// </value>
        [ConfigurationProperty(RoleOrUserAttribute, IsRequired = true)]
        public string RoleOrUser
        {
            get { return Convert.ToString(this[RoleOrUserAttribute]); }
            set { this[RoleOrUserAttribute] = value; }
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