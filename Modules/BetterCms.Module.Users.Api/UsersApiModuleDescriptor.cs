using Autofac;

using BetterCms.Core.Modules;

using BetterCms.Module.Api.Operations.Users.Roles;
using BetterCms.Module.Api.Operations.Users.Roles.Role;
using BetterCms.Module.Api.Operations.Users.Users;
using BetterCms.Module.Api.Operations.Users.Users.User;
using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;

using BetterCms.Module.Users.Api.Operations.Users.Roles;
using BetterCms.Module.Users.Api.Operations.Users.Roles.Role;
using BetterCms.Module.Users.Api.Operations.Users.Users;
using BetterCms.Module.Users.Api.Operations.Users.Users.User;
using BetterCms.Module.Users.Api.Operations.Users.Users.User.Validate;

using BetterModules.Core.Modules.Registration;

namespace BetterCms.Module.Users.Api
{
    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class UsersApiModuleDescriptor : CmsModuleDescriptor
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
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public override int Order
        {
            get
            {
                return int.MaxValue - 50;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UsersService>().As<IUsersService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ValidateUserService>().As<IValidateUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<RolesService>().As<IRolesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
        }
    }
}
