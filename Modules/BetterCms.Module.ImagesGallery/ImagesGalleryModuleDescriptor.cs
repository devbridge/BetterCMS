using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.ImagesGallery.Content.Resources;
using BetterCms.Module.ImagesGallery.Registration;
using BetterCms.Module.Root;

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
        /// The images gallery java script module descriptor.
        /// </summary>
        private readonly ImagesGalleryJsModuleIncludeDescriptor imagesGalleryJsModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesGalleryModuleDescriptor" /> class.
        /// </summary>
        public ImagesGalleryModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
            imagesGalleryJsModuleIncludeDescriptor = new ImagesGalleryJsModuleIncludeDescriptor(this);
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
        /// Gets known client side modules in page module.
        /// </summary>        
        /// <returns>List of known client side modules in page module.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new[] { imagesGalleryJsModuleIncludeDescriptor };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>List of page action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new SeparatorProjection(9999),
                    new LinkActionProjection(imagesGalleryJsModuleIncludeDescriptor, page => "loadSiteSettingsAlbums")
                        {
                            Order = 9999,
                            Title = () => ImagesGalleryGlobalization.SiteSettings_AlbumsMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration)
                        }
                };
        }
    }
}
