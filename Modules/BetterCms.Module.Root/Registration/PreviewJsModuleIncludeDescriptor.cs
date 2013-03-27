using System.Web;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Controllers;

namespace BetterCms.Module.Root.Registration
{
    public class PreviewJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public PreviewJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.preview")
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