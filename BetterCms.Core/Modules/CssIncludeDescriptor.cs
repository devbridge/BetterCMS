using System;

using BetterModules.Core.Exceptions;
using BetterModules.Core.Modules;
using BetterModules.Core.Web.Mvc.Extensions;

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
        /// <param name="module">The container module.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="minFileName">Name of the minified CSS file version.</param>
        /// <param name="isPublic">if set to <c>true</c> then this CSS include is public (visible for in the edit/non-edit mode).</param>
        public CssIncludeDescriptor(CmsModuleDescriptor module, string fileName, string minFileName = null, bool isPublic = false)
        {
            if (isPublic && string.IsNullOrEmpty(minFileName))
            {
                throw new CoreException("Public CSS includes should describe a minified file version itself.", new ArgumentNullException("minFileName", "Please define the minFileName parameter."));
            }

            ContainerModule = module;                        
            IsPublic = isPublic;
            Path = VirtualPath.Combine(module.CssBasePath, fileName);

            // If minFileName is not given then CMS will load it from a bcms.[module-name].min.css file.
            if (!string.IsNullOrEmpty(minFileName))
            {
                MinPath = VirtualPath.Combine(module.CssBasePath, minFileName);
            }
        }

        /// <summary>
        /// Gets or sets the server side container module.
        /// </summary>
        /// <value>
        /// The container module.
        /// </value>
        public CmsModuleDescriptor ContainerModule { get; private set; }

        /// <summary>
        /// Gets or sets the CSS include path (like '/file/bcms-pages/content/styles/bcms.page.css').
        /// </summary>
        /// <value>
        /// The js module path.
        /// </value>
        public string Path { get; private set; }

        /// <summary>
        /// Gets path of the minified CSS file if it was provided.
        /// </summary>
        /// <value>
        /// The path of the minified CSS file if it was provided.
        /// </value>
        public string MinPath { get; private set; }

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
