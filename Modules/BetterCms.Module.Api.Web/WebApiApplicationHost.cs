using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Modules.Registration;
using BetterCms.Module.Api.Filters;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;

using Common.Logging;

using ServiceStack.ServiceInterface.Validation;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace BetterCms.Module.Api
{
	public class WebApiApplicationHost
		: AppHostBase
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
            List<Assembly> assemblies = new List<Assembly>();
            IModulesRegistration modulesRegistry;
            IAssemblyLoader assemblyLoader = null;

            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                modulesRegistry = container.Resolve<IModulesRegistration>();
                if (modulesRegistry == null)
                {
                    throw new CmsApiException("Failed to resolve IModulesRegistration.");
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