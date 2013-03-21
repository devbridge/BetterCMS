using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// Security module descriptor.
    /// </summary>
    public class SecurityJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityJavaScriptModuleDescriptor"/> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public SecurityJavaScriptModuleDescriptor(ModuleDescriptor containerModule) : base(containerModule, "bcms.security")
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