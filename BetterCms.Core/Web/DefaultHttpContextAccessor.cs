using System.Web;

namespace BetterCms.Core.Web
{
    /// <summary>
    /// Default implementation of http context accessor. Provides HttpContext.Current context wrapper.
    /// </summary>
    public class DefaultHttpContextAccessor : IHttpContextAccessor
    {
        /// <summary>
        /// Gets the current http context.
        /// </summary>
        /// <returns>Current http context instance.</returns>
        public HttpContextBase GetCurrent()
        {
            var httpContext = HttpContext.Current;
            
            if (httpContext == null)
            {
                return null;
            }            

            return new HttpContextWrapper(httpContext); 
        }

        /// <summary>
        /// Returns the physical file path that corresponds to the specified virtual path on the Web server.
        /// </summary>
        /// <param name="path">The virtual path of the Web server.</param>
        /// <returns>
        /// The physical file path that corresponds to path.
        /// </returns>
        public string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}
