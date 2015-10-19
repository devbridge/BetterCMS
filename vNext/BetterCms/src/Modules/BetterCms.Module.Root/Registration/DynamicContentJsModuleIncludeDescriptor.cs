using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicContentJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public DynamicContentJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms.dynamicContent")
        {

            Links = new IActionUrlProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "failedLoadDialogMessage", () => RootGlobalization.DynamicContent_FailedLoadDialog_Message, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "dialogLoadingCancelledMessage", () => RootGlobalization.DynamicContent_DialogLoadingCancelled_Message, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "forbiddenDialogMessage", () => RootGlobalization.DynamicContent_DialogForbidden_Message, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "unauthorizedDialogMessage", () => RootGlobalization.DynamicContent_DialogUnauthorized_Message, loggerFactory)
                };
        }
    }
}