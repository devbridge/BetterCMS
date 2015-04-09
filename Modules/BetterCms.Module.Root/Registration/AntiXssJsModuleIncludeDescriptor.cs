using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Registration
{
    public class AntiXssJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public AntiXssJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.antiXss")
        {
            Links = new IActionProjection[] {};

            Globalization = new IActionProjection[]
            {
                new JavaScriptModuleGlobalization(this, "antiXssContainsHtmlError", () => RootGlobalization.AntiXss_Contains_Html_Error)
            };
        }
    }
}