using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Viddler.Content.Resources;
using BetterCms.Module.Viddler.Controllers;

namespace BetterCms.Module.Viddler.Registration
{
    /// <summary>
    /// bcms.viddler.videos.js module descriptor.
    /// </summary>
    public class VideosJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideosJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public VideosJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.viddler.videos")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<VideosController>(this, "uploadVideoDialogUrl", c => c.UploadVideos(null, null)),
                    new JavaScriptModuleLinkTo<VideosController>(this, "getUploadDataUrl", c => c.GetViddlerDataForUpload()),
                    new JavaScriptModuleLinkTo<VideosController>(this, "saveUploadedVideosUrl", c => c.SaveVideos(null)),
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "uploadVideoDialogTitle", () => ViddlerGlobalization.VideoUploadDialog_Title),
                    new JavaScriptModuleGlobalization(this, "uploadVideoDialogSaveButtonTitle", () => ViddlerGlobalization.VideoUploadDialog_SaveButton_Title),
                };
        }
    }
}