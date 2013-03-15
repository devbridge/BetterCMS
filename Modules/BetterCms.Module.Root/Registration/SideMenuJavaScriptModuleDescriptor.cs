using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Registration
{
    public class SidemenuJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public SidemenuJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.sidemenu")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {  
                     new JavaScriptModuleGlobalization(this, "stickRightMenuTitle", () => RootGlobalization.Sidebar_Footer_Right_DragTitle),
                     new JavaScriptModuleGlobalization(this, "stickLeftMenuTitle", () => RootGlobalization.Sidebar_Footer_Left_DragTitle)
                };
        }
    }
}