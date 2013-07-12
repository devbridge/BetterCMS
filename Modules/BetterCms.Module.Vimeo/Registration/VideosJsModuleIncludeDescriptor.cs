using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Vimeo.Content.Resources;
using BetterCms.Module.Vimeo.Controllers;

namespace BetterCms.Module.Vimeo.Registration
{
    /// <summary>
    /// bcms.vimeo.videos.js module descriptor.
    /// </summary>
    public class VideosJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideosJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public VideosJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.vimeo.videos")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<VideosController>(this, "selectVideoDialogUrl", c => c.Search(null)),
                    new JavaScriptModuleLinkTo<VideosController>(this, "saveSelectedVideosUrl", c => c.SaveVideos(null)),
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "selectVideoDialogTitle", () => VimeoGlobalization.VideoSelectionDialog_Title),
                    new JavaScriptModuleGlobalization(this, "selectVideoDialogSaveButtonTitle", () => VimeoGlobalization.VideoSelectionDialog_SaveButtonTitle),
                };
        }
    }
}