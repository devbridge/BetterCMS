using Autofac;

using BetterCms.Configuration;
using BetterCms.Core.Modules;
using BetterCms.Core.Services.Storage;

using BetterModules.Core.Modules.Registration;

namespace BetterCms.Module.WindowsAzureStorage
{
    /// <summary>
    /// A storage module based on the Windows Azure Storage cloud service.
    /// </summary>
    public class WindowsAzureStorageModuleDescriptor : CmsModuleDescriptor
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
                return "WindowsAzureStorage";
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
                return "A storage module based on Windows Azure Storage cloud service.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsAzureStorageModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public WindowsAzureStorageModuleDescriptor(ICmsConfiguration configuration)
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
            if (Configuration.Storage.ServiceType == StorageServiceType.Auto)
            {
                // TODO: check if Storage module is registered if throw cmsexception.                
                containerBuilder.RegisterType<WindowsAzureStorageService>().As<IStorageService>().SingleInstance();
            }
        }
    }
}
