using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Registration
{
    public class KnockoutExtendersJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public KnockoutExtendersJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.ko.extenders")
        {
            Links = new IActionProjection[]
                {   
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "maximumLengthMessage", () => RootGlobalization.Validation_MaximumLengthExceeded_Message), 
                    new JavaScriptModuleGlobalization(this, "requiredFieldMessage", () => RootGlobalization.Validation_FieldIsRequired_Message), 
                };
        }
    }
}