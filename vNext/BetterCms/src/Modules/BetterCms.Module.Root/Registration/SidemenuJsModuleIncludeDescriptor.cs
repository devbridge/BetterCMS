using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    public class SidemenuJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public SidemenuJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms.sidemenu")
        {

            Links = new IActionUrlProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {  
                     new JavaScriptModuleGlobalization(this, "stickRightMenuTitle", () => RootGlobalization.Sidebar_Footer_Right_DragTitle, loggerFactory),
                     new JavaScriptModuleGlobalization(this, "stickLeftMenuTitle", () => RootGlobalization.Sidebar_Footer_Left_DragTitle, loggerFactory)
                };
        }
    }
}