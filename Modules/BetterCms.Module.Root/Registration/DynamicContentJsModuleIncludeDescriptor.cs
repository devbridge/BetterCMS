using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicContentJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public DynamicContentJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.dynamicContent")
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