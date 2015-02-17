using Autofac;

using Devbridge.Platform.Core.Configuration;
using Devbridge.Platform.Core.DataAccess;
using Devbridge.Platform.Core.DataAccess.DataContext;
using Devbridge.Platform.Core.DataAccess.DataContext.Fetching;
using Devbridge.Platform.Core.DataAccess.DataContext.Migrations;
using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Environment.Assemblies;
using Devbridge.Platform.Core.Environment.FileSystem;
using Devbridge.Platform.Core.Exceptions;
using Devbridge.Platform.Core.Modules.Registration;
using Devbridge.Platform.Core.Security;

namespace Devbridge.Platform.Core
{
    /// <summary>
    /// Application Context Container
    /// </summary>
    public static class ApplicationContext
    {
        private static readonly object configurationLoaderLock = new object();

        private static volatile IConfiguration config;

        private static volatile bool configLoaded;

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        /// <value>
        /// The application configuration.
        /// </value>
        public static IConfiguration Config
        {
            get
            {
                if (!configLoaded)
                {
                    lock (configurationLoaderLock)
                    {
                        if (!configLoaded)
                        {
                            IConfigurationLoader configurationLoader = new DefaultConfigurationLoader();
                            config = configurationLoader.TryLoadConfig<DefaultConfigurationSection>();

                            configLoaded = true;
                        }
                    }
                    configLoaded = true;
                }

                return config;
            }
        }

        /// <summary>
        /// Creates the configured application dependencies container.
        /// </summary>
        /// <returns>The container builder.</returns>
        public static ContainerBuilder InitializeContainer(ContainerBuilder builder = null)
        {
            if (builder == null)
            {
                builder = new ContainerBuilder();
            }

            builder.RegisterType<DefaultModulesRegistration>().As<IModulesRegistration>().SingleInstance();
            builder.RegisterType<DefaultSessionFactoryProvider>().As<ISessionFactoryProvider>().SingleInstance();
            builder.RegisterType<DefaultAssemblyLoader>().As<IAssemblyLoader>().SingleInstance();
            builder.RegisterType<DefaultUnitOfWorkFactory>().As<IUnitOfWorkFactory>().SingleInstance();
            builder.RegisterType<DefaultMappingResolver>().As<IMappingResolver>().SingleInstance();
            builder.RegisterType<DefaultWorkingDirectory>().As<IWorkingDirectory>().SingleInstance();
            builder.RegisterType<DefaultFetchingProvider>().As<IFetchingProvider>().SingleInstance();
            builder.RegisterType<DefaultUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultRepository>().As<IRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultVersionChecker>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DefaultMigrationRunner>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DefaultPrincipalProvider>().As<IPrincipalProvider>().SingleInstance();
            builder.RegisterType<DefaultAssemblyManager>().As<IAssemblyManager>().SingleInstance();

            if (Config != null)
            {
                builder.RegisterInstance(Config).As<IConfiguration>().SingleInstance();
            }

            return builder;
        }

        /// <summary>
        /// Loads available assemblies.
        /// </summary>
        public static void LoadAssemblies()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                if (container == null)
                {
                    throw new PlatformException("Application dependencies container is not initialized.");
                }

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
