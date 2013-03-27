using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class HtmlEditorJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public HtmlEditorJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.htmlEditor")
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