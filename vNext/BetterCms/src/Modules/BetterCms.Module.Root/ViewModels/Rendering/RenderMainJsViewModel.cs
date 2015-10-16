using System;
using System.Collections.Generic;

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
        /// Gets or sets a value indicating whether debug mode is on.
        /// </summary>
        /// <value>
        /// <c>true</c> if debug mode is on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDebugMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use *.min.js references.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use *.min.js references; otherwise, <c>false</c>.
        /// </value>
        public bool UseMinReferences { get; set; }

        /// <summary>
        /// Gets or sets the CMS version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("IsDebugMode: {0}", IsDebugMode);
        }
    }
}