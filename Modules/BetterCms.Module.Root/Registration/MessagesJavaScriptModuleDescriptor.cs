using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Registration
{
    public class MessagesJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public MessagesJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.messages")
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