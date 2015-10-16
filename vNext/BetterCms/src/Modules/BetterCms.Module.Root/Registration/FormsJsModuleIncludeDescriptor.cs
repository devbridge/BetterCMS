using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    public class FormsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public FormsJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.forms")
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