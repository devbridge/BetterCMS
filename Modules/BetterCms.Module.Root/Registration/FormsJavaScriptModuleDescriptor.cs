using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class FormsJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public FormsJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.forms")
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