using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class TooltipJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public TooltipJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.tooltip")
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