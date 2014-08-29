using System;

using Autofac;

using BetterCms.Configuration;
using BetterCms.Core.Modules;
using BetterCms.Core.Services.Caching;

namespace BetterCms.Module.AppFabricCache
{
    /// <summary>
    /// Caching module based on AppFabric cache server.
    /// </summary>
    public class AppFabricCacheModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleId = "c7980cec-fd62-4e1a-84a4-1ecfe99283e0";

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public override Guid Id
        {
            get
            {
                return new Guid(ModuleId);
            }
        }

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
                return "AppFabricCache";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return "Caching module based on AppFabric cache server.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppFabricCacheModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AppFabricCacheModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            if (Configuration.Cache.CacheType == CacheServiceType.Auto)
            {
                containerBuilder.RegisterType<AppFabricCacheService>().As<ICacheService>().SingleInstance();
            }
        }
    }
}
