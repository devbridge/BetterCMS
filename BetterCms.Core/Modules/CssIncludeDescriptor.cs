using BetterCms.Core.Mvc.Extensions;

namespace BetterCms.Core.Modules
{
    /// <summary>
    /// Describes a CSS file include.
    /// </summary>
    public class CssIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CssIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        /// <param name="fileName">Name of the file.</param>
        public CssIncludeDescriptor(ModuleDescriptor containerModule, string fileName)
        {
            ContainerModule = containerModule;            
            Path = VirtualPath.Combine(containerModule.CssBasePath, fileName);            
        }

        /// <summary>
        /// Gets or sets the server side container module.
        /// </summary>
        /// <value>
        /// The container module.
        /// </value>
        public ModuleDescriptor ContainerModule { get; private set; }

        /// <summary>
        /// Gets or sets the CSS include path (like '/file/bcms-pages/content/styles/bcms.page.css').
        /// </summary>
        /// <value>
        /// The js module path.
        /// </value>
        public string Path { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Path;
        }
    }
}
