using System;

using BetterCms.Core.Modules;

namespace BetterCms.Module.ImagesGallery
{
    /// <summary>
    /// Images gallery module descriptor.
    /// </summary>
    public class ImagesGalleryModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "images_gallery";

        /// <summary>
        /// The images gallery area name.
        /// </summary>
        internal const string ImagesGalleryAreaName = "bcms-images-gallery";

        /// <summary>
        /// The images gallery schema name
        /// </summary>
        internal const string ImagesGallerySchemaName = "bcms_images_gallery";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesGalleryModuleDescriptor" /> class.
        /// </summary>
        public ImagesGalleryModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
        }

        internal const string ModuleId = "abbdb3e7-818c-457c-a787-98822ffafc61";

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
                return "An images gallery module for Better CMS.";
            }
        }

        /// <summary>
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public override string AreaName
        {
            get
            {
                return ImagesGalleryAreaName;
            }
        }

        /// <summary>
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public override string SchemaName
        {
            get
            {
                return ImagesGallerySchemaName;
            }
        }
    }
}
