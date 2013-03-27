using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// Security module descriptor.
    /// </summary>
    public class SecurityJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityJsModuleIncludeDescriptor"/> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public SecurityJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.security")
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