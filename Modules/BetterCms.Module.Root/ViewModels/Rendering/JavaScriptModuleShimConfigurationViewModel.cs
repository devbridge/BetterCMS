using System.Collections.Generic;

namespace BetterCms.Module.Root.ViewModels.Rendering
{
    public class JavaScriptModuleShimConfigurationViewModel
    {
        public string Exports { get; set; }

        public List<string> Depends { get; set; }

        public override string ToString()
        {
            return string.Format("Exports: '{0}', NumberOfDepends: {1}", Exports, Depends != null ? Depends.Count : 0);
        }
    }
}