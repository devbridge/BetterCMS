using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicContentJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public DynamicContentJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.dynamicContent", "/file/bcms-root/scripts/bcms.dynamicContent")
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