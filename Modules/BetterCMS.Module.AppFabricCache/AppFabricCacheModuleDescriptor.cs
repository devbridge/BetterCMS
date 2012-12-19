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
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            if (configuration.Cache.CacheType == CacheServiceType.Auto)
            {
                containerBuilder.RegisterType<AppFabricCacheService>().As<ICacheService>().SingleInstance();
            }
        }
    }
}
