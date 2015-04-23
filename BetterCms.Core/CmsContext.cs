using Autofac;

using BetterCms.Configuration;
using BetterCms.Core.Environment.Host;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Registration;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Storage;

using BetterModules.Core.Configuration;
using BetterModules.Core.Dependencies;
using BetterModules.Core.Modules.Registration;
using BetterModules.Core.Web;
using BetterModules.Core.Web.Configuration;
using BetterModules.Core.Web.Environment.Host;
using BetterModules.Core.Web.Modules.Registration;

using System;

using BetterModules.Core.Web.Services.Caching;

namespace BetterCms.Core
{
    /// <summary>
    /// BetterCMS context container.
    /// </summary>
    public static class CmsContext
    {
        private static bool isStarted;

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
                            ICmsConfigurationLoader configurationLoader = new CmsConfigurationLoader();
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
        [Obsolete("Explicit host registration is deprecated and not required anymore.")]
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
        public static ContainerBuilder InitializeContainer(ContainerBuilder builder = null)
        {
            if (builder == null)
            {
                builder = new ContainerBuilder();
            }

            if (isStarted)
            {
                return builder;
            }

            builder = WebApplicationContext.InitializeContainer(builder, Config);
            builder.RegisterType<DefaultTextEncryptor>().As<ITextEncryptor>().SingleInstance();

            builder.RegisterType<CmsModulesRegistration>()
                .As<IModulesRegistration>()
                .As<IWebModulesRegistration>()
                .As<ICmsModulesRegistration>()
                .SingleInstance();

            builder.RegisterInstance(Config)
                .As<ICmsConfiguration>()
                .As<IWebConfiguration>()
                .As<IConfiguration>()
                .SingleInstance();

            builder.RegisterType<DefaultCmsHost>()
                .As<IWebApplicationHost>()
                .As<ICmsHost>()
                .SingleInstance();

            RegisterCacheService(builder);
            RegisterStorageService(builder);

            isStarted = true;

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
    }
}