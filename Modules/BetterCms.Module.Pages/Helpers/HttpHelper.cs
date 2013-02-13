namespace BetterCms.Module.Pages.Helpers
{
    /// <summary>
    /// Helper class for http related tasks.
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// Virtual the path exists.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Returns true if file exists or false otherwise.</returns>
        public static bool VirtualPathExists(string virtualPath)
        {
            if (!string.IsNullOrWhiteSpace(virtualPath))
            {
                return System.Web.Hosting.HostingEnvironment.VirtualPathProvider.FileExists(virtualPath);
            }
            
            return false;
        }
    }
}