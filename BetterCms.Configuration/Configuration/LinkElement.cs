// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkElement.cs" company="Devbridge Group LLC">
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