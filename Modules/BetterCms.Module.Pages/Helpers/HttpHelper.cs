using System.IO;

namespace BetterCms.Module.Pages.Helpers
{
    public static class HttpHelper
    {
        /// <summary>
        /// Virtuals the path exists.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        public static bool VirtualPathExists(string virtualPath)
        {
            var exists = false;

            if (!string.IsNullOrWhiteSpace(virtualPath))
            {
                var fileName = System.Web.HttpContext.Current.Server.MapPath(virtualPath);
                exists = File.Exists(fileName);
            }
            
            return exists;
        }
    }
}