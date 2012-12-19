using Autofac;

using BetterCms.Core.Modules;

namespace BetterCms.Module.AmazonS3Storage
{
    /// <summary>
    /// A storage module based on the Amazon S3 cloud service.
    /// </summary>
    public class AmazonS3StorageModuleDescriptor : ModuleDescriptor
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
        /// <param name="configuration">The configuration.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
        }
    }
}
