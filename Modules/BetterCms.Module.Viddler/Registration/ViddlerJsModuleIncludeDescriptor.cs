using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Viddler.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class ViddlerJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViddlerJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public ViddlerJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.viddler")
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