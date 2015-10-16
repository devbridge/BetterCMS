using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    public class TooltipJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public TooltipJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.tooltip")
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