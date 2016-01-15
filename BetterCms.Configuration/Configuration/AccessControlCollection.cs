// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControlCollection.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    /// <summary>
    /// Custom roles collections for security configuration.
    /// </summary>
    public class AccessControlCollection : ConfigurationElementCollection
    {
        private const string DefaultAccessLevelAttribute = "defaultAccessLevel";

        /// <summary>
        /// Gets the content encryption key.
        /// </summary>
        /// <value>
        /// The content encryption key.
        /// </value>
        [ConfigurationProperty(DefaultAccessLevelAttribute, IsRequired = false, DefaultValue = "ReadWrite")]
        public string DefaultAccessLevel
        {
            get { return Convert.ToString(this[DefaultAccessLevelAttribute]); }
            set { this[DefaultAccessLevelAttribute] = value; }
        }

        #region Indexers

        /// <summary>
        /// Gets or sets a property, attribute, or child element of this configuration element.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Configuration element.</returns>
        public AccessControlElement this[int index]
        {
            get
            {
                return (AccessControlElement)BaseGet(index);
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
        /// Adds the specified element to collection.
        /// </summary>
        /// <param name="element">The element.</param>
        public void Add(AccessControlElement element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// Gets the element by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Configuration element.</returns>
        public AccessControlElement GetElementByKey(string key)
        {
            return (AccessControlElement)BaseGet(key);
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
            return new AccessControlElement();
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
            return ((AccessControlElement)element).Identity;
        }

        #endregion
    }
}