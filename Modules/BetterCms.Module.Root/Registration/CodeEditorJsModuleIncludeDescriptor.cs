using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class CodeEditorJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public CodeEditorJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.codeEditor")
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