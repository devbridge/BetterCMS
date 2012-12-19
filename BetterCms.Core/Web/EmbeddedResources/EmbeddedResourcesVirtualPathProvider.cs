using System;
using System.Collections;
using System.Linq;
using System.Web.Caching;
using System.Web.Hosting;

namespace BetterCms.Core.Web.EmbeddedResources
{
    /// <summary>
    /// Embedded resources virtual path provider.
    /// </summary>
    public class EmbeddedResourcesVirtualPathProvider : VirtualPathProvider
    {
        /// <summary>
        /// Embedded resources provider contract.
        /// </summary>
        private readonly IEmbeddedResourcesProvider embeddedResourcesProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourcesVirtualPathProvider" /> class.
        /// </summary>
        /// <param name="embeddedResourcesProvider">The embedded resources provider.</param>
        public EmbeddedResourcesVirtualPathProvider(IEmbeddedResourcesProvider embeddedResourcesProvider)
        {
            this.embeddedResourcesProvider = embeddedResourcesProvider;            
        }

        /// <summary>
        /// Creates a cache dependency based on the specified virtual paths.
        /// </summary>
        /// <param name="virtualPath">The path to the primary virtual resource.</param>
        /// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource.</param>
        /// <param name="utcStart">The UTC time at which the virtual resources were read.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Caching.CacheDependency" /> object for the specified virtual resources.
        /// </returns>
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (embeddedResourcesProvider.IsEmbeddedResourceVirtualPath(virtualPath))
            {
                return null;
            }

            string[] strArray = (from s in virtualPathDependencies.OfType<string>()
                                 where !embeddedResourcesProvider.IsEmbeddedResourceVirtualPath(s)
                                 select s).ToArray<string>();

            return base.GetCacheDependency(virtualPath, strArray, utcStart);
        }

        /// <summary>
        /// Returns a cache key to use for the specified virtual path.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual resource.</param>
        /// <returns>
        /// A cache key for the specified virtual resource.
        /// </returns>
        public override string GetCacheKey(string virtualPath)
        {
            return null;
        }

        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        public override bool FileExists(string virtualPath)
        {
            bool isEmbeddedResourceVirtualPath = embeddedResourcesProvider.IsEmbeddedResourceVirtualPath(virtualPath);

            if (isEmbeddedResourceVirtualPath)
            {
                return true;
            }
           
            return base.FileExists(virtualPath);
        }

        /// <summary>
        /// Gets a virtual file from the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// A descendent of the <see cref="T:System.Web.Hosting.VirtualFile" /> class that represents a file in the virtual file system.
        /// </returns>
        public override VirtualFile GetFile(string virtualPath)
        {            
            VirtualFile embeddedResourcesVirtualFile = embeddedResourcesProvider.GetEmbeddedResourceVirtualFile(virtualPath);
            
            if (embeddedResourcesVirtualFile != null)
            {
                return embeddedResourcesVirtualFile;
            }            

            return base.GetFile(virtualPath);
        }
    }
}
