using System;
using System.Collections.Generic;

using BetterCms.Core.Modules.Projections;

namespace BetterCms.Core.Modules
{
    /// <summary>
    /// Describes a java script module.
    /// </summary>
    public class JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        /// <param name="name">The module name.</param>
        /// <param name="fileName">A name of the module file.</param>
        public JavaScriptModuleDescriptor(ModuleDescriptor containerModule, string name, string fileName = null)
        {
            ContainerModule = containerModule;
            Name = name;
            Links = new List<IActionProjection>();
            Globalization = new List<IActionProjection>();

            Path = System.IO.Path.Combine(containerModule.BaseScriptPath, fileName ?? name);
            if (Path != null && Path.EndsWith(".js"))
            {
                Path = Path.Substring(0, Path.LastIndexOf(".js", StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Gets or sets the server side container module.
        /// </summary>
        /// <value>
        /// The container module.
        /// </value>
        public ModuleDescriptor ContainerModule { get; set; }

        /// <summary>
        /// Gets or sets the name of the java script module (like bcms.page).
        /// </summary>
        /// <value>
        /// The name of the java script module.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the 'friendly' module name to use internally in java script.
        /// </summary>
        /// <value>
        /// The 'friendly' module name to use internally.
        /// </value>
        public string FriendlyName
        {
            get
            {
                // TODO: replace with more intelligent logic.
                return Name.Replace(".", string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the js module path (like '/file/bcms-pages/scripts/bcms.page').
        /// </summary>
        /// <value>
        /// The js module path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the url list.
        /// </summary>
        /// <value>
        /// The urls.
        /// </value>
        public IList<IActionProjection> Links { get; set; }

        /// <summary>
        /// Gets or sets the js globalization.
        /// </summary>
        /// <value>
        /// The js globalization.
        /// </value>
        public IList<IActionProjection> Globalization { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
