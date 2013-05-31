using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;

namespace BetterCms.Core.Web.DynamicHtmlLayout
{
    /// <summary>
    /// Dynamic html layout virtual path provider.
    /// </summary>
    public class DynamicHtmlLayoutVirtualPathProvider : VirtualPathProvider
    {
        /// <summary>
        /// Dynamic html layout provider contract.
        /// </summary>
        private readonly IDynamicHtmlLayoutProvider dynamicHtmlLayoutProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicHtmlLayoutVirtualPathProvider" /> class.
        /// </summary>
        /// <param name="dynamicHtmlLayoutProvider">The dynamic html layout provider.</param>
        public DynamicHtmlLayoutVirtualPathProvider(IDynamicHtmlLayoutProvider dynamicHtmlLayoutProvider)
        {
            this.dynamicHtmlLayoutProvider = dynamicHtmlLayoutProvider;            
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
            if (dynamicHtmlLayoutProvider.IsDynamicHtmlLayoutVirtualPath(virtualPath))
            {
                return null;
            }

            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
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
            bool isDynamicHtmlLayoutVirtualPath = dynamicHtmlLayoutProvider.IsDynamicHtmlLayoutVirtualPath(virtualPath);

            if (isDynamicHtmlLayoutVirtualPath)
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
            VirtualFile dynamicHtmlLayoutVirtualFile = dynamicHtmlLayoutProvider.GetDynamicHtmlLayoutVirtualFile(virtualPath);

            if (dynamicHtmlLayoutVirtualFile != null)
            {
                return dynamicHtmlLayoutVirtualFile;
            }            

            return base.GetFile(virtualPath);
        }
    }
}
