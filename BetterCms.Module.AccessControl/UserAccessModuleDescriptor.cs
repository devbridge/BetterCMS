using Autofac;

using BetterCms.Core.Modules;

namespace BetterCms.Module.AccessControl
{
    /// <summary>
    /// UserAccess module descriptor.
    /// </summary>
    public class UserAccessModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "UserAccess";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccessModuleDescriptor" /> class.
        /// </summary>
        public UserAccessModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of pages module.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
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
                return "User access module for Better CMS.";
            }
        }
        
        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AccessControlService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
