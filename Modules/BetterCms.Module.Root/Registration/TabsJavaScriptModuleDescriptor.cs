using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root
{
    public class TabsJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public TabsJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.tabs")
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