using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class BcmsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public BcmsJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms")
        {

            Links = new IActionUrlProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "sessionHasExpired", () => RootGlobalization.Message_SessionExpiredLoginToContinue_Message, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "failedToProcessRequest", () => RootGlobalization.Message_FailedToProcessRequest_Message, loggerFactory)
                };
        }
    }
}