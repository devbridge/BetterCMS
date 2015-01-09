using System.Collections.Generic;

namespace BetterCms.Core.Modules
{
    public class JsShimConfigDescriptor
    {
        public string Exports { get; set; }

        public List<string> Depends { get; set; }
    }
}
