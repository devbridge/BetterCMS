// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiModuleDescriptor.cs" company="Devbridge Group LLC">
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
using System.Web;

using Autofac;

using BetterCms.Core.Modules;

using BetterModules.Core.Dependencies;
using BetterModules.Core.Web.Dependencies;
using BetterModules.Core.Web.Modules.Registration;
using BetterModules.Events;

namespace BetterCms.Module.Api
{
    using Common.Logging;

    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class WebApiModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "web-api";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiModuleDescriptor" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public WebApiModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
            WebCoreEvents.Instance.HostStart += ApplicationStart;
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of API module.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "An Web API module for Better CMS.";
            }
        }
       
        /// <summary>
        /// Registers module custom routes.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterCustomRoutes(WebModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            context.IgnoreRoute("bcms-api/{*pathInfo}");
        }

        private void ApplicationStart(SingleItemEventArgs<HttpApplication> args)
        {
            Logger.Info("OnHostStart: preparing web api...");

            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var containerProvider = container.Resolve<PerWebRequestContainerProvider>();
                new WebApiApplicationHost(() => containerProvider.CurrentScope).Init();
            }

            Logger.Info("OnHostStart: preparing web api completed.");
        }
    }
}
