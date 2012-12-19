using System.Collections.Generic;
using System.Reflection;

namespace BetterCms.Core.Web.EmbeddedResources
{
    /// <summary>
    /// Defines the contract to manage embedded resources.
    /// </summary>
    public interface IEmbeddedResourcesProvider
    {
        /// <summary>
        /// Scans and adds an embedded resources from assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        void AddEmbeddedResourcesFrom(Assembly assembly);

        /// <summary>
        /// Checks if virtual path exists as embedded resource.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>
        ///   <c>true</c> if virtual path is embedded resource path; otherwise, <c>false</c>.
        /// </returns>
        bool IsEmbeddedResourceVirtualPath(string virtualPath);

        /// <summary>
        /// Gets the embedded resource virtual file.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Embedded resource virtual file.</returns>
        EmbeddedResourcesVirtualFile GetEmbeddedResourceVirtualFile(string virtualPath);

        /// <summary>
        /// Gets the embedded resource JavaScript virtual files.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>List of embedded resource virtual files.</returns>
        IEnumerable<EmbeddedResourcesVirtualFile> GetEmbeddedResourceJsVirtualFiles(Assembly assembly);
    }
}
