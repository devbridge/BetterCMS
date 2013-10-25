namespace BetterCms.Core.Web.DynamicLayouts
{
    /// <summary>
    /// Defines the contract to manage dynamic html layout contents.
    /// </summary>
    public interface IDynamicHtmlLayoutProvider
    {
        /// <summary>
        /// Checks if virtual path exists as dynamic html layout.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>
        ///   <c>true</c> if virtual path is dynamic html layout path; otherwise, <c>false</c>.
        /// </returns>
        bool IsDynamicHtmlLayoutVirtualPath(string virtualPath);

        /// <summary>
        /// Gets the dynamic html layout virtual file.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Dynamic html layout virtual file.</returns>
        DynamicHtmlLayoutVirtualFile GetDynamicHtmlLayoutVirtualFile(string virtualPath);
    }
}