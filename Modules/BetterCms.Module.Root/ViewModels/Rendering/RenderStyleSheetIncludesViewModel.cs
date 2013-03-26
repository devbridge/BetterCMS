using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Modules;

namespace BetterCms.Module.Root.ViewModels.Rendering
{
    public class RenderStyleSheetIncludesViewModel
    {
        public RenderStyleSheetIncludesViewModel()
        {
            StyleSheetFiles = new List<string>();
        }

        public RenderStyleSheetIncludesViewModel(IEnumerable<CssIncludeDescriptor> styleSheetFiles)
        {
            StyleSheetFiles = styleSheetFiles.Select(f => f.Path).ToList();
        }

        public IEnumerable<string> StyleSheetFiles { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {            
            return string.Join(", ", StyleSheetFiles);
        }
    }
}