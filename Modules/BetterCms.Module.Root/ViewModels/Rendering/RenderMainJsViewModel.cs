using System.Collections.Generic;

using BetterCms.Module.Root.Models.Rendering;

namespace BetterCms.Module.Root.ViewModels.Rendering
{
    /// <summary>
    /// View model for dynamic java script file (main.js) initialization.
    /// </summary>
    public class RenderMainJsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderMainJsViewModel" /> class.
        /// </summary>
        public RenderMainJsViewModel()
        {
            JavaScriptModules = new List<JavaScriptModuleViewModel>();
        }

        /// <summary>
        /// Gets or sets a list of JS modules.
        /// </summary>
        /// <value>
        /// A list of JS modules.
        /// </value>
        public IEnumerable<JavaScriptModuleViewModel> JavaScriptModules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether error trace is enabled in client side.
        /// </summary>
        /// <value>
        /// <c>true</c> if error trace enabled in client side; otherwise, <c>false</c>.
        /// </value>
        public bool EnableClientSideErrorTrace { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("EnabledClientSideErrorTrace: {0}", EnableClientSideErrorTrace);
        }
    }
}