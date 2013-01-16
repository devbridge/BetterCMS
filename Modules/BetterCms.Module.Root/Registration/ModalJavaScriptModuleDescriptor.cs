using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

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
                    new JavaScriptModuleGlobalization(this, "save", () => RootGlobalization.Button_Save), 
                    new JavaScriptModuleGlobalization(this, "cancel", () => RootGlobalization.Button_Cancel), 
                    new JavaScriptModuleGlobalization(this, "ok", () => RootGlobalization.Button_Ok)
                };
        }
    }
}