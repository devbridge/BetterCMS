using System.Collections.Generic;
using System.Linq;

namespace BetterCms.Module.Root.ViewModels.Rendering
{
    public class RenderStyleSheetIncludesViewModel
    {
        public IEnumerable<string> StyleSheetFiles { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {            
            return string.Join(", ", StyleSheetFiles ?? Enumerable.Empty<string>());
        }
    }
}