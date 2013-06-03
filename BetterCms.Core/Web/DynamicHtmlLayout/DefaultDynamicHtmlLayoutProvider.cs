namespace BetterCms.Core.Web.DynamicHtmlLayout
{
    /// <summary>
    /// Default implementation of dynamic html layouts provider.
    /// </summary>
    public class DefaultDynamicHtmlLayoutProvider : IDynamicHtmlLayoutProvider
    {
        /// <summary>
        /// Checks if virtual path exists as dynamic html layout.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>
        /// <c>true</c> if virtual path is dynamic html layout path; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDynamicHtmlLayoutVirtualPath(string virtualPath)
        {
            return virtualPath.Contains(DynamicHtmlLayoutContentsContainer.DynamicHtmlLayoutVirtualPath);
        }

        /// <summary>
        /// Gets the dynamic html layout virtual file.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Dynamic html layout virtual file.</returns>
        public DynamicHtmlLayoutVirtualFile GetDynamicHtmlLayoutVirtualFile(string virtualPath)
        {
            if (IsDynamicHtmlLayoutVirtualPath(virtualPath))
            {
                return new DynamicHtmlLayoutVirtualFile(virtualPath);
            }

            return null;
        }
    }
}