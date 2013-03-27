using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class TabsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public TabsJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.tabs")
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