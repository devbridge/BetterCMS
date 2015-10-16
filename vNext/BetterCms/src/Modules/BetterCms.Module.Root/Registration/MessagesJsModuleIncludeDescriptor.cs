using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    public class MessagesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public MessagesJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.messages")
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