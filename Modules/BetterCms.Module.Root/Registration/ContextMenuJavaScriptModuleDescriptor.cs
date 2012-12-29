using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class ContextMenuJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public ContextMenuJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.contextMenu", "/file/bcms-root/scripts/bcms.contextMenu")
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