using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.ImagesGallery.Content.Resources;
using BetterCms.Module.ImagesGallery.Controllers;

namespace BetterCms.Module.ImagesGallery.Registration
{
    /// <summary>
    /// bcms.images.gallery.js module descriptor.
    /// </summary>
    public class ImagesGalleryJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesGalleryJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public ImagesGalleryJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.images.gallery")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<AlbumController>(this, "loadSiteSettingsAlbumsUrl", c => c.ListTemplate()),
                    new JavaScriptModuleLinkTo<AlbumController>(this, "loadAlbumsUrl", c => c.AlbumsList(null)),
                    new JavaScriptModuleLinkTo<AlbumController>(this, "saveAlbumUrl", c => c.SaveAlbum(null)),
                    new JavaScriptModuleLinkTo<AlbumController>(this, "deleteAlbumUrl", c => c.DeleteAlbum(null, null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "deleteAlbumDialogTitle", () => ImagesGalleryGlobalization.DeleteAlbum_Confirmation_Message)
                };
        }
    }
}