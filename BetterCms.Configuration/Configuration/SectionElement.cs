// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SectionElement.cs" company="Devbridge Group LLC">
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
    [ConfigurationCollection(typeof(LinkCollection), AddItemName = LinksNodeName, CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class SectionElement : ConfigurationElementCollection
    {
        private const string NameAttribute = "name";
        private const string LinksNodeName = "links";

        #region Properties

        /// <summary>
        /// Gets or sets the name.
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
        /// Gets the links.
        /// </summary>
        [ConfigurationProperty(LinksNodeName, IsRequired = true)]
        public LinkCollection Links
        {
            get { return (LinkCollection)base[LinksNodeName]; }
        }

        #endregion

        #region Indexers

        public LinkCollection this[int index]
        {
            get
            {
                return (LinkCollection)BaseGet(index);
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

        #region Overrides

        protected override ConfigurationElement CreateNewElement()
        {
            return new LinkElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as LinkElement).Name;
        }

        protected override string ElementName
        {
            get { return "section"; }
        }

        #endregion
    }
}