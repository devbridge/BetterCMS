using Autofac;

using BetterCms.Core.Modules;

using BetterCms.Module.Api.Operations.Users;
using BetterCms.Module.Users.Api.Operations.Users;

namespace BetterCms.Module.Users.Api
{
    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class UsersApiModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersApiModuleDescriptor" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public UsersApiModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
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
                return "users-api";
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
                return "An Users API module for Better CMS.";
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultUsersApiOperations>().As<IUsersApiOperations>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
        }
    }
}
