using System.Collections.Generic;
using System.Linq;

namespace BetterCms.Module.Root.ViewModels.Rendering
{
    /// <summary>
    /// View model for dynamic java script file (main.js) initialization.
    /// </summary>
    public class RenderProcessorJsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderMainJsViewModel" /> class.
        /// </summary>
        public RenderProcessorJsViewModel()
        {
            JavaScriptModules = new List<JavaScriptModuleInclude>();
        }

        /// <summary>
        /// Gets or sets a list of JS modules.
        /// </summary>
        /// <value>
        /// A list of JS modules.
        /// </value>
        public IEnumerable<JavaScriptModuleInclude> JavaScriptModules { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("JavaScriptModules: {0}", string.Join(", ", JavaScriptModules != null ? JavaScriptModules.Select(f => f.ToString()) : Enumerable.Empty<string>()));
        }
    }
}