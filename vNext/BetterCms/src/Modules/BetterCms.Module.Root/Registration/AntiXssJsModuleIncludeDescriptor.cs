using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    public class AntiXssJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public AntiXssJsModuleIncludeDescriptor(CmsModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms.antiXss")
        {
            Links = new IActionUrlProjection[] {};

            Globalization = new IActionProjection[]
            {
                new JavaScriptModuleGlobalization(this, "antiXssContainsHtmlError", () => RootGlobalization.AntiXss_Contains_Html_Error, loggerFactory)
            };
        }
    }
}