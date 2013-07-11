using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Vimeo.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class VimeoJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VimeoJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public VimeoJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.vimeo")
        {
            Links = new IActionProjection[]
                {
                };

            Globalization = new IActionProjection[]
                {
                };
        }
    }
}