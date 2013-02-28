using System.Web;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Controllers;

namespace BetterCms.Module.Root.Registration
{
    public class PreviewJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public PreviewJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.preview", "/file/bcms-root/scripts/bcms.preview")
        {           
            Links = new IActionProjection[]
                {   
                    new JavaScriptModuleLinkTo<PreviewController>(this, "previewPageUrl", controller => controller.Index("{0}", "{1}"), true)                   
                };

            Globalization = new IActionProjection[]
                {                    
                };
        }
    }
}