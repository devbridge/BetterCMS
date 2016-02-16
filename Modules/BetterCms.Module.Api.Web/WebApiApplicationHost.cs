// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiApplicationHost.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Reflection;

using Autofac;

using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Modules.Registration;

using BetterCms.Module.Api.Filters;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;

using Common.Logging;

using BetterModules.Core.Dependencies;
using BetterModules.Core.Environment.Assemblies;

using ServiceStack.ServiceInterface.Validation;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace BetterCms.Module.Api
{
    public class WebApiApplicationHost : AppHostBase
    {
        private readonly Func<ILifetimeScope> containerProvider;

        private static readonly ILog logger = LogManager.GetCurrentClassLogger();

        public WebApiApplicationHost(Func<ILifetimeScope> containerProvider)
            : base("Better CMS Web API Host", GetAssembliesWithServices())
        {
            this.containerProvider = containerProvider;
        }

        public override void Configure(Funq.Container container)
        {
            RequestBinders.Clear();

            JsConfig.EmitCamelCaseNames = true;
            JsConfig.IncludeNullValues = true;
            JsConfig.DateHandler = JsonDateHandler.ISO8601;

            Plugins.Add(new ValidationFeature());

            // Add custom request filter
            RequestFilters.Add(GetRequestProcessor.DeserializeJsonFromGet);

            container.Adapter = new AutofacContainerAdapter(containerProvider);
            container.RegisterValidators(typeof(GetTagRequestValidator).Assembly);
        }

        private static Assembly[] GetAssembliesWithServices()
        {
            var assemblies = new List<Assembly>();
            ICmsModulesRegistration modulesRegistry;
            IAssemblyLoader assemblyLoader = null;

            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                modulesRegistry = container.Resolve<ICmsModulesRegistration>();
                if (modulesRegistry == null)
                {
                    throw new CmsApiException("Failed to resolve ICmsModulesRegistration.");
                }

                assemblyLoader = container.Resolve<IAssemblyLoader>();
                if (assemblyLoader == null)
                {
                    throw new CmsApiException("Failed to resolve IAssemblyLoader.");
                }

                foreach (var module in modulesRegistry.GetModules())
                {
                    try
                    {
                        var assembly = assemblyLoader.Load(module.ModuleDescriptor.AssemblyName);
                        if (assembly != null)
                        {
                            var types = assemblyLoader.GetLoadableTypes(assembly);
                            foreach (var type in types)
                            {
                                if (typeof(ServiceStack.ServiceInterface.Service).IsAssignableFrom(type) && type != null && type.IsPublic && type.IsClass
                                    && !type.IsAbstract)
                                {
                                    assemblies.Add(assembly);
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorFormat("Failed to check for ServiceStack services in the assembly {0}.", ex, module.ModuleDescriptor.AssemblyName);
                    }
                }
            }

            return assemblies.ToArray();
        }
    }
}