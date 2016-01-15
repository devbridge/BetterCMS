// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleSiteSearchModuleDescriptor.cs" company="Devbridge Group LLC">
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
using Autofac;

using BetterCMS.Module.GoogleSiteSearch.Services.Search;

using BetterCms;
using BetterCms.Core.Modules;
using BetterCms.Module.Search.Services;

using BetterModules.Core.Modules.Registration;

namespace BetterCMS.Module.GoogleSiteSearch
{
    public class GoogleSiteSearchModuleDescriptor : CmsModuleDescriptor
    {
        internal const string ModuleName = "googlesitesearch";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        public override string Description
        {
            get
            {
                return "The Google Site Search integration module for Better CMS.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleSiteSearchModuleDescriptor" class./>
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public GoogleSiteSearchModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
        }
        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<GoogleSiteSearchService>().As<ISearchService>().SingleInstance();
            containerBuilder.RegisterType<DefaultWebClient>().As<IWebClient>().SingleInstance();
        }
    }
}
