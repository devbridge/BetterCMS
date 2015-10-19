using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;
using BetterModules.Core.Web.Mvc.Extensions;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    public class PreviewJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public PreviewJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory, IControllerExtensions controllerExtensions)
            : base(module, "bcms.preview")
        {           
            Links = new IActionUrlProjection[]
                {   
                    new JavaScriptModuleLinkTo<PreviewController>(this, "previewPageUrl", controller => controller.Index("{0}", "{1}"), loggerFactory, controllerExtensions, true)                   
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "closeButtonTitle", () => RootGlobalization.Button_Close, loggerFactory), 
                };
        }
    }
}