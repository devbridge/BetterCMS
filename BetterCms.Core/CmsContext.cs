using System;
using System.Security.Principal;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

using Autofac;
using Autofac.Core;

using BetterCms.Api;
using BetterCms.Configuration;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Environment.FileSystem;
using BetterCms.Core.Environment.Host;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Modules.Registration;
using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Mvc.Extensions;
using BetterCms.Core.Mvc.Routes;
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
        /// <summary>
        /// Creates the data API.
        /// </summary>
        /// <typeparam name="TApiContext">The type of the API content.</typeparam>
        /// <returns></returns>
        public static TApiContext CreateApiContextOf<TApiContext>(ApiContext parentApiContext = null) where TApiContext : ApiContext
        {
            ILifetimeScope lifetimeScope;

            if (parentApiContext == null)
            {
                lifetimeScope = ContextScopeProvider.CreateChildContainer();
            }
            else
            {
                lifetimeScope = parentApiContext.GetLifetimeScope();
            }

            if (!lifetimeScope.IsRegistered<TApiContext>())
            {
                throw new CmsApiException(string.Format("A '{0}' type is unknown as Better CMS API context.", typeof(TApiContext).Name));
            }

            var apiContext = lifetimeScope.Resolve<TApiContext>(new Parameter[] { new PositionalParameter(0, lifetimeScope) });

            if (parentApiContext != null)
            {
                apiContext.MarkParentLifetimeScope();
            }

            return apiContext;                        
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
            IConfigurationLoader configurationLoader = new DefaultConfigurationLoader();
            ICmsConfiguration cmsConfiguration = configurationLoader.LoadCmsConfiguration();

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterInstance(configurationLoader).As<IConfigurationLoader>().SingleInstance();
            builder.RegisterInstance(cmsConfiguration).As<ICmsConfiguration>().SingleInstance();

            builder.RegisterType<DefaultCmsHost>().As<ICmsHost>().SingleInstance();
            builder.RegisterType<ApiContext>().AsSelf().SingleInstance();
            builder.RegisterType<DefaultSessionFactoryProvider>().As<ISessionFactoryProvider>().SingleInstance();
            builder.RegisterType<DefaultAssemblyLoader>().As<IAssemblyLoader>().SingleInstance();
            builder.RegisterType<DefaultAssemblyManager>().As<IAssemblyManager>().SingleInstance();
            builder.RegisterType<DefaultUnitOfWorkFactory>().As<IUnitOfWorkFactory>().SingleInstance();

            builder.RegisterType<DefaultModulesRegistration>().As<IModulesRegistration>().SingleInstance();
            builder.RegisterType<DefaultMappingResolver>().As<IMappingResolver>().SingleInstance();
            builder.RegisterType<DefaultWorkingDirectory>().As<IWorkingDirectory>().SingleInstance();
            builder.RegisterType<DefaultCmsControllerFactory>().SingleInstance();
            builder.RegisterType<DefaultEmbeddedResourcesProvider>().As<IEmbeddedResourcesProvider>().SingleInstance();
            builder.RegisterType<DefaultHttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultControllerExtensions>().As<IControllerExtensions>().SingleInstance();
            builder.RegisterType<DefaultCommandResolver>().As<ICommandResolver>().InstancePerLifetimeScope();

            builder.RegisterType<DefaultUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultRepository>().As<IRepository>().InstancePerLifetimeScope();
            
            builder.RegisterInstance(new DefaultRouteTable(RouteTable.Routes)).As<IRouteTable>().SingleInstance();
            
            builder.RegisterType<PerWebRequestContainerProvider>().InstancePerLifetimeScope();
          
            builder.RegisterType<DefaultMigrationRunner>().AsImplementedInterfaces().SingleInstance();
            
            RegisterCacheService(cmsConfiguration, builder);

            RegisterStorageService(cmsConfiguration, builder);

            return builder;
        }       

        /// <summary>
        /// Registers the cache service.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="builder">The builder.</param>
        /// <exception cref="BetterCms.Core.Exceptions.CmsException">Failed to register cache service.</exception>
        private static void RegisterCacheService(ICmsConfiguration cmsConfiguration, ContainerBuilder builder)
        {
            try
            {
                if (cmsConfiguration.Cache.CacheType != CacheServiceType.Custom)
                {
                    builder.RegisterType<HttpRuntimeCacheService>().As<ICacheService>().SingleInstance();
                }
                else
                {
                    string customCacheTypeName = cmsConfiguration.Cache.GetValue("typeName");
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

        private static void RegisterStorageService(ICmsConfiguration cmsConfiguration, ContainerBuilder builder)
        {
            try
            {
                if (cmsConfiguration.Storage.ServiceType == StorageServiceType.FileSystem || cmsConfiguration.Storage.ServiceType == StorageServiceType.Auto)
                {                    
                    builder.RegisterType<FileSystemStorageService>().As<IStorageService>().SingleInstance();
                }
                else if (cmsConfiguration.Storage.ServiceType == StorageServiceType.Ftp)
                {
                    builder.RegisterType<FtpStorageService>().As<IStorageService>().SingleInstance();                    
                }
                else
                {
                    string customStorageTypeName = cmsConfiguration.Storage.GetValue("typeName");
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
