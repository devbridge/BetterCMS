using System.Collections.Generic;

namespace BetterCms.Module.Root.Models.Rendering
{
    public class RenderStyleSheetIncludesViewModel
    {
        public RenderStyleSheetIncludesViewModel()
        {
            StyleSheetFiles = new List<string>();
        }

        public RenderStyleSheetIncludesViewModel(IEnumerable<string> styleSheetFiles)
        {
            StyleSheetFiles = styleSheetFiles;
        }

        public IEnumerable<string> StyleSheetFiles { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            // TODO: cannot add any key from object
            return string.Empty;
        }
    }
}