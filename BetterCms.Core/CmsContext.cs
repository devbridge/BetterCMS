using System;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

using Autofac;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Environment.FileSystem;
using BetterCms.Core.Environment.Host;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Registration;
using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Mvc.Extensions;
using BetterCms.Core.Mvc.Routes;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Caching;
using BetterCms.Core.Services.Storage;
using BetterCms.Core.Web;
using BetterCms.Core.Web.EmbeddedResources;
using BetterCms.Core.Web.ViewEngines;

namespace BetterCms.Core
{
    /// <summary>
    /// BetterCMS context container.
    /// </summary>
    public static class CmsContext
    {
        private static readonly object configurationLoaderLock = new object();

        private static volatile ICmsConfiguration config;

        /// <summary>
        /// Gets the Better CMS configuration.
        /// </summary>
        /// <value>
        /// The Better CMS configuration.
        /// </value>
        public static ICmsConfiguration Config 
        {
            get
            {
                if (config == null)
                {
                    lock (configurationLoaderLock)
                    {
                        if (config == null)
                        {
                            IConfigurationLoader configurationLoader = new DefaultConfigurationLoader();
                            config = configurationLoader.LoadCmsConfiguration();                            
                        }
                    }
                }

                return config;
            }
        }
        /// <summary>
        /// Constructs the host context.
        /// </summary>
        /// <returns>Constructed host context.</returns>
        public static ICmsHost RegisterHost()
        {
            ICmsHost cmsHost;
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                if (container == null)
                {
                    throw new CmsException("Better CMS dependencies container is not initialized.");
                }

                cmsHost = container.Resolve<ICmsHost>();                
                if (cmsHost == null)
                {
                    throw new CmsException("Better CMS host context was not created.");
                }
            }

            return cmsHost;
        }

        /// <summary>
        /// Creates the configured BetterCMS root dependencies container.
        /// </summary>
        /// <returns>The container builder.</returns>
        public static ContainerBuilder InitializeContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            
            builder.RegisterInstance(Config).As<ICmsConfiguration>().SingleInstance();

            builder.RegisterType<DefaultCmsHost>().As<ICmsHost>().SingleInstance();
            
            builder.RegisterType<DefaultSessionFactoryProvider>().As<ISessionFactoryProvider>().SingleInstance();
            builder.RegisterType<DefaultAssemblyLoader>().As<IAssemblyLoader>().SingleInstance();
            builder.RegisterType<DefaultAssemblyManager>().As<IAssemblyManager>().SingleInstance();
            builder.RegisterType<DefaultUnitOfWorkFactory>().As<IUnitOfWorkFactory>().SingleInstance();
            builder.RegisterType<DefaultTextEncryptor>().As<ITextEncryptor>().SingleInstance();

            builder.RegisterType<DefaultModulesRegistration>().As<IModulesRegistration>().SingleInstance();
            builder.RegisterType<DefaultMappingResolver>().As<IMappingResolver>().SingleInstance();
            builder.RegisterType<DefaultWorkingDirectory>().As<IWorkingDirectory>().SingleInstance();
            builder.RegisterType<DefaultCmsControllerFactory>().SingleInstance();
            builder.RegisterType<DefaultEmbeddedResourcesProvider>().As<IEmbeddedResourcesProvider>().SingleInstance();
            builder.RegisterType<DefaultHttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultControllerExtensions>().As<IControllerExtensions>().SingleInstance();
            builder.RegisterType<DefaultCommandResolver>().As<ICommandResolver>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultFetchingProvider>().As<IFetchingProvider>().SingleInstance();

            builder.RegisterType<DefaultUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultRepository>().As<IRepository>().InstancePerLifetimeScope();
            
            builder.RegisterInstance(new DefaultRouteTable(RouteTable.Routes)).As<IRouteTable>().SingleInstance();
            
            builder.RegisterType<PerWebRequestContainerProvider>().InstancePerLifetimeScope();
          
            builder.RegisterType<DefaultVersionChecker>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DefaultMigrationRunner>().AsImplementedInterfaces().SingleInstance();

            RegisterCacheService(builder);

            RegisterStorageService(builder);

            return builder;
        }       

        /// <summary>
        /// Registers the cache service.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <exception cref="BetterCms.Core.Exceptions.CmsException">Failed to register cache service.</exception>
        private static void RegisterCacheService(ContainerBuilder builder)
        {
            try
            {
                if (Config.Cache.CacheType != CacheServiceType.Custom)
                {
                    builder.RegisterType<HttpRuntimeCacheService>().As<ICacheService>().SingleInstance();
                }
                else
                {
                    string customCacheTypeName = Config.Cache.GetValue("typeName");
                    if (!string.IsNullOrEmpty(customCacheTypeName))
                    {
                        Type customCacheType = Type.GetType(customCacheTypeName);
                        if (customCacheType == null)
                        {
                            throw new CmsException(string.Format("Failed to register a cache service. A specified type '{0}' was not found.", customCacheTypeName));
                        }
                        
                        if (typeof(ICacheService).IsAssignableFrom(customCacheType))
                        {
                            builder.RegisterType(customCacheType).As<ICacheService>().SingleInstance();
                        }
                        else
                        {
                            throw new CmsException(string.Format("Failed to register a cache service. Specified type {0} is not inherited from the {1} interface.", customCacheTypeName, typeof(ICacheService).FullName));
                        }
                    }
                    else
                    {
                        throw new CmsException(
                            "Failed to register a cache service. The type name of the custom cache service is not specified. Please add to cms.config under cache section <add key=\"typeName\" value=\"your.full.custom.type.name\" />");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CmsException("Failed to register a cache service.", ex);
            }
        }

        private static void RegisterStorageService(ContainerBuilder builder)
        {
            try
            {
                if (Config.Storage.ServiceType == StorageServiceType.FileSystem || Config.Storage.ServiceType == StorageServiceType.Auto)
                {                    
                    builder.RegisterType<FileSystemStorageService>().As<IStorageService>().SingleInstance();
                }
                else if (Config.Storage.ServiceType == StorageServiceType.Ftp)
                {
                    builder.RegisterType<FtpStorageService>().As<IStorageService>().SingleInstance();                    
                }
                else
                {
                    string customStorageTypeName = Config.Storage.GetValue("typeName");
                    if (!string.IsNullOrEmpty(customStorageTypeName))
                    {
                        Type customStorageType = Type.GetType(customStorageTypeName);
                        if (customStorageType == null)
                        {
                            throw new CmsException(string.Format("Failed to register a storage service. A specified type '{0}' was not found.", customStorageTypeName));
                        }

                        if (typeof(IStorageService).IsAssignableFrom(customStorageType))
                        {
                            builder.RegisterType(customStorageType).As<IStorageService>().SingleInstance();
                        }
                        else
                        {
                            throw new CmsException(string.Format("Failed to register a storage service. Specified type {0} is not inherited from the {1} interface.", customStorageTypeName, typeof(IStorageService).FullName));
                        }
                    }
                    else
                    {
                        throw new CmsException(
                            "Failed to register a storage service. The type name of the custom storage service is not specified. Please add to cms.config under cache section <add key=\"typeName\" value=\"your.full.custom.type.name\" />");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CmsException("Failed to register a storage service.", ex);
            }
        }

        /// <summary>
        /// Loads available assemblies.
        /// </summary>
        internal static void LoadAssemblies()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                if (container == null)
                {
                    throw new CmsException("Better CMS dependencies container is not initialized.");
                }

                if (HostingEnvironment.IsHosted)
                {
                    HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourcesVirtualPathProvider(container.Resolve<IEmbeddedResourcesProvider>()));
                }

                ControllerBuilder.Current.SetControllerFactory(container.Resolve<DefaultCmsControllerFactory>());
                ViewEngines.Engines.Insert(0, new EmbeddedResourcesViewEngine());

                IAssemblyManager assemblyManager = container.Resolve<IAssemblyManager>();
                                
                // First add referenced modules...
                assemblyManager.AddReferencedModules();
                
                // ...then scan and register uploaded modules.
                assemblyManager.AddUploadedModules();

                var moduleRegistration = container.Resolve<IModulesRegistration>();
                moduleRegistration.InitializeModules();
            }
        }
    }
}
