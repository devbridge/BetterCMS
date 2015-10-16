using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class CustomValidationJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public CustomValidationJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.customValidation")
        {
            Links = new IActionProjection[] {};

            Globalization = new IActionProjection[] { };
        }
    }
}