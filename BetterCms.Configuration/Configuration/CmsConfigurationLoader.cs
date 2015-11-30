// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CmsConfigurationLoader.cs" company="Devbridge Group LLC">
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
using System.Configuration;
using System.Web.Configuration;

using BetterModules.Core.Configuration;

namespace BetterCms.Configuration
{
    public class CmsConfigurationLoader : DefaultConfigurationLoader, ICmsConfigurationLoader
    {
        public ICmsConfiguration LoadCmsConfiguration()
        {
            // NOTE: LoadConfig<CmsConfigurationSection> fails to load storage and menu section elements. May be because they are of type ConfigurationElementCollection and not ConfigurationElement
            // return LoadConfig<CmsConfigurationSection>();
            return LoadCmsConfigurationInternal();
        }

        private static ICmsConfiguration LoadCmsConfigurationInternal()
        {
            var config = ConfigurationManager.GetSection("cms");
            return (ICmsConfiguration)config;
        }

        protected override System.Configuration.Configuration OpenApplicationConfiguration()
        {
            return WebConfigurationManager.OpenWebConfiguration("~/Web.config");
        }
    }
}
