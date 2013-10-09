using System.Threading;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Extensions;

namespace BetterCms.Module.Root.Registration
{
    public class DatePickerJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public DatePickerJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.datepicker")
        {

            Links = new IActionProjection[]
                { 
                    new JavaScriptModuleLink(this, "calendarImageUrl", VirtualPath.Combine(module.CssBasePath, "images", "icn-calendar.png"))
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "dateFormat", () => Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower().Replace("yy", "y")),
                    new JavaScriptModuleGlobalization(this, "currentCulture", () => Thread.CurrentThread.CurrentCulture.Name),
                };
        }
    }
}