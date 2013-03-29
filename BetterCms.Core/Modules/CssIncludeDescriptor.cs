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
        /// <param name="isPublic">if set to <c>true</c> then this CSS include is public (visible for in the edit/non-edit mode).</param>
        public CssIncludeDescriptor(ModuleDescriptor containerModule, string fileName, bool isPublic = false)
        {
            ContainerModule = containerModule;            
            Path = VirtualPath.Combine(containerModule.CssBasePath, fileName);
            IsPublic = isPublic;
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
        /// Gets a value indicating whether this CSS include is public.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this CSS include is public (visible for in the edit/non-edit mode); otherwise, <c>false</c>.
        /// </value>
        public bool IsPublic { get; private set; }

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
