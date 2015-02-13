using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

using Autofac;

using Devbridge.Platform.Core.Configuration;
using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Environment.Assemblies;
using Devbridge.Platform.Core.Exceptions;
using Devbridge.Platform.Core.Modules.Registration;
using Devbridge.Platform.Core.Security;
using Devbridge.Platform.Core.Web.Configuration;
using Devbridge.Platform.Core.Web.Dependencies;
using Devbridge.Platform.Core.Web.Environment.Assemblies;
using Devbridge.Platform.Core.Web.Environment.Host;
using Devbridge.Platform.Core.Web.Modules;
using Devbridge.Platform.Core.Web.Modules.Registration;
using Devbridge.Platform.Core.Web.Mvc;
using Devbridge.Platform.Core.Web.Mvc.Commands;
using Devbridge.Platform.Core.Web.Mvc.Extensions;
using Devbridge.Platform.Core.Web.Mvc.Routes;
using Devbridge.Platform.Core.Web.Security;
using Devbridge.Platform.Core.Web.Services.Caching;
using Devbridge.Platform.Core.Web.Web;
using Devbridge.Platform.Core.Web.Web.EmbeddedResources;

using RazorGenerator.Mvc;

namespace Devbridge.Platform.Core.Web
{
    /// <summary>
    /// Web Application Context Container
    /// </summary>
    public static class WebApplicationContext
    {
        private static readonly object configurationLoaderLock = new object();

        private static volatile IWebConfiguration config;
        
        private static volatile bool configLoaded;

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        /// <value>
        /// The application configuration.
        /// </value>
        public static IWebConfiguration Config
        {
            get
            {
                if (!configLoaded)
                {
                    lock (configurationLoaderLock)
                    {
                        if (!configLoaded)
                        {
                            IConfigurationLoader configurationLoader = new DefaultWebConfigurationLoader();
                            config = configurationLoader.TryLoadConfig<DefaultWebConfigurationSection>();

                            if (config == null)
                            {
                                config = new DefaultWebConfigurationSection();
                            }

                            configLoaded = true;
                        }
                    }
                    configLoaded = true;
                }

                return config;
            }
        }

        /// <summary>
        /// Constructs the host context.
        /// </summary>
        /// <returns>Constructed host context.</returns>
        public static IWebApplicationHost RegisterHost()
        {
            IWebApplicationHost host;
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                if (container == null)
                {
                    throw new PlatformException("Web application dependencies container is not initialized.");
                }

                host = container.Resolve<IWebApplicationHost>();
                if (host == null)
                {
                    throw new PlatformException("Web application host context was not created.");
                }
            }

            return host;
        }

        /// <summary>
        /// Creates the configured web application dependencies container.
        /// </summary>
        /// <returns>The container builder.</returns>
        public static ContainerBuilder InitializeContainer(ContainerBuilder builder = null)
        {
            if (builder == null)
            {
                builder = new ContainerBuilder();
            }

            builder = ApplicationContext.InitializeContainer(builder);

            builder.RegisterType<DefaultWebModulesRegistration>()
                .As<IModulesRegistration>()
                .As<IWebModulesRegistration>()
                .SingleInstance();

            builder.RegisterType<DefaultWebControllerFactory>().SingleInstance();
            builder.RegisterType<DefaultEmbeddedResourcesProvider>().As<IEmbeddedResourcesProvider>().SingleInstance();
            builder.RegisterType<DefaultHttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultControllerExtensions>().As<IControllerExtensions>().SingleInstance();
            builder.RegisterType<DefaultCommandResolver>().As<ICommandResolver>().InstancePerLifetimeScope();
            builder.RegisterInstance(new DefaultRouteTable(RouteTable.Routes)).As<IRouteTable>().SingleInstance();
            builder.RegisterType<PerWebRequestContainerProvider>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultWebPrincipalProvider>().As<IPrincipalProvider>().SingleInstance();
            builder.RegisterType<DefaultWebAssemblyManager>().As<IAssemblyManager>().SingleInstance();
            builder.RegisterType<HttpRuntimeCacheService>().As<ICacheService>().SingleInstance();
            builder.RegisterType<DefaultWebApplicationHost>().As<IWebApplicationHost>().SingleInstance();

            if (Config != null)
            {
                builder.RegisterInstance(Config)
                    .As<IConfiguration>()
                    .As<IWebConfiguration>()
                    .SingleInstance();
            }

            return builder;
        }

        /// <summary>
        /// Loads available assemblies.
        /// </summary>
        public static void LoadAssemblies()
        {
            ApplicationContext.LoadAssemblies();

            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                if (HostingEnvironment.IsHosted)
                {
                    HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourcesVirtualPathProvider(container.Resolve<IEmbeddedResourcesProvider>()));
                }
                else
                {
                    throw new PlatformException("Failed to register EmbeddedResourcesVirtualPathProvider as a virtual path provider.");
                }

                ControllerBuilder.Current.SetControllerFactory(container.Resolve<DefaultWebControllerFactory>());

                // Register precompiled views for all the assemblies
                var precompiledAssemblies = new List<PrecompiledViewAssembly>();

                var moduleRegistration = container.Resolve<IModulesRegistration>();
                moduleRegistration.GetModules().Select(m => m.ModuleDescriptor).Distinct().ToList().ForEach(
                    descriptor =>
                    {
                        var webDescriptor = descriptor as WebModuleDescriptor;
                        if (webDescriptor != null)
                        {
                            var precompiledAssembly = new PrecompiledViewAssembly(descriptor.GetType().Assembly, string.Format("~/Areas/{0}/", webDescriptor.AreaName))
                            {
                                UsePhysicalViewsIfNewer = false
                            };
                            precompiledAssemblies.Add(precompiledAssembly);
                        }
                    });

                var engine = new CompositePrecompiledMvcEngine(precompiledAssemblies.ToArray());
                ViewEngines.Engines.Insert(0, engine);
                VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
            }
        }
    }
}
