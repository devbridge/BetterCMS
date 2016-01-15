// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CmsInstallationConfigurationElement.cs" company="Devbridge Group LLC">
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
