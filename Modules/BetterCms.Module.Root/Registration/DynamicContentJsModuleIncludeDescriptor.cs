using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;

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
                    new JavaScriptModuleGlobalization(this, "failedLoadDialogMessage", () => RootGlobalization.DynamicContent_FailedLoadDialog_Message), 
                    new JavaScriptModuleGlobalization(this, "dialogLoadingCancelledMessage", () => RootGlobalization.DynamicContent_DialogLoadingCancelled_Message), 
                    new JavaScriptModuleGlobalization(this, "forbiddenDialogMessage", () => RootGlobalization.DynamicContent_DialogForbidden_Message),
                    new JavaScriptModuleGlobalization(this, "unauthorizedDialogMessage", () => RootGlobalization.DynamicContent_DialogUnauthorized_Message),
                };
        }
    }
}