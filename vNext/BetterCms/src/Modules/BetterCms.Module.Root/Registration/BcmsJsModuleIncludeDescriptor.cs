using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class BcmsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public BcmsJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "sessionHasExpired", () => RootGlobalization.Message_SessionExpiredLoginToContinue_Message), 
                    new JavaScriptModuleGlobalization(this, "failedToProcessRequest", () => RootGlobalization.Message_FailedToProcessRequest_Message),
                };
        }
    }
}