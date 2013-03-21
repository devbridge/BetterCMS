using System.Configuration;

namespace BetterCms.Configuration
{
    /// <summary>
    /// Custom roles collections for security configuration.
    /// </summary>
    public class CustomRolesCollection : ConfigurationElementCollection
    {
        #region Indexers

        /// <summary>
        /// Gets or sets a property, attribute, or child element of this configuration element.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Configuration element.</returns>
        public CustomRoleElement this[int index]
        {
            get
            {
                return (CustomRoleElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        #endregion

        /// <summary>
        /// Gets the element by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Configuration element.</returns>
        public CustomRoleElement GetElementByKey(string key)
        {
            return (CustomRoleElement)BaseGet(key);
        }

        #region Overrides

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CustomRoleElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CustomRoleElement)element).Permission;
        }

        #endregion
    }
}