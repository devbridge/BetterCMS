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
                    new JavaScriptModuleLinkTo<AlbumController>(this, "editAlbumUrl", c => c.EditAlbum("{0}")),
                    new JavaScriptModuleLinkTo<AlbumController>(this, "createAlbumUrl", c => c.CreateAlbum()),
                    new JavaScriptModuleLinkTo<AlbumController>(this, "selectAlbumUrl", c => c.SelectAlbum()),
                    new JavaScriptModuleLinkTo<AlbumController>(this, "deleteAlbumUrl", c => c.DeleteAlbum(null, null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "editAlbumDialogTitle", () => ImagesGalleryGlobalization.EditAlbum_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "createAlbumDialogTitle", () => ImagesGalleryGlobalization.CreateAlbum_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "deleteAlbumDialogTitle", () => ImagesGalleryGlobalization.DeleteAlbum_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "selectAlbumDialogTitle", () => ImagesGalleryGlobalization.SelectAlbum_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "selectAlbumDialogAcceptButton", () => ImagesGalleryGlobalization.SelectAlbum_Dialog_AcceptButton),
                    new JavaScriptModuleGlobalization(this, "albumNotSelectedMessage", () => ImagesGalleryGlobalization.SelectAlbum_NotSelected_Message),
                };
        }
    }
}