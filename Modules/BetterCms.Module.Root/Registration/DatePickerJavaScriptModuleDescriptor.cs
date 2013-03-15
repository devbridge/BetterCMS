using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class DatePickerJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public DatePickerJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.datepicker")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                };
        }
    }
}