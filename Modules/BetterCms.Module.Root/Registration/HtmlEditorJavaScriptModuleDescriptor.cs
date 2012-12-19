using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class HtmlEditorJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public HtmlEditorJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.htmlEditor", "/file/bcms-root/scripts/bcms.htmlEditor")
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