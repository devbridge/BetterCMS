using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class ModalJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public ModalJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.modal", "/file/bcms-root/scripts/bcms.modal")
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