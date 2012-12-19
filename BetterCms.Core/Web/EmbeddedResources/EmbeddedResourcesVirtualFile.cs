using System.IO;
using System.Reflection;
using System.Web.Hosting;

namespace BetterCms.Core.Web.EmbeddedResources
{
    /// <summary>
    /// Embedded resources virtual file.
    /// </summary>
    public class EmbeddedResourcesVirtualFile : VirtualFile
    {
        /// <summary>
        /// Embedded resources provider contract.
        /// </summary>
        private readonly Assembly assembly;

        /// <summary>
        /// Name of the embedded resource.
        /// </summary>
        private readonly string resourceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourcesVirtualFile" /> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the embedded resource.</param>
        /// <param name="virtualPath">The virtual path.</param>
        public EmbeddedResourcesVirtualFile(Assembly assembly, string resourceName, string virtualPath)
            : base(virtualPath)
        {
            this.assembly = assembly;
            this.resourceName = resourceName;
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open()
        {
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
