using Autofac;

using BetterCms.Configuration;
using BetterCms.Core.Modules;
using BetterCms.Core.Services.Storage;

namespace BetterCms.Module.AmazonS3Storage
{
    /// <summary>
    /// A storage module based on the Amazon S3 cloud service.
    /// </summary>
    public class AmazonS3StorageModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonS3StorageModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AmazonS3StorageModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
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
                return "AmazonS3Storage";
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
                return "A storage module based on the Amazon S3 cloud service.";
            }
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
                containerBuilder.RegisterType<AmazonS3StorageService>().As<IStorageService>().SingleInstance();
            }
        }
    }
}
