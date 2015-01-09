using System.Collections.Generic;

namespace BetterCms.Module.Root.ViewModels.Rendering
{
    public class JavaScriptModuleShimConfigurationViewModel
    {
        public string Exports { get; set; }

        public List<string> Depends { get; set; }
    }
}