using System.Reflection;

namespace BetterCms.Core.Web.EmbeddedResources
{
    /// <summary>
    /// Describes embedded assembly resource.
    /// </summary>
    public class EmbeddedResourceDescriptor
    {
        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>
        /// The name of the resource.
        /// </value>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>
        public AssemblyName AssemblyName { get; set; }
    }
}
