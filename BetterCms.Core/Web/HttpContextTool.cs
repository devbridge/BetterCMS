using System;
using System.Web;

namespace BetterCms.Core.Web
{
    /// <summary>
    /// TODO: Move to helpers
    /// </summary>
    public class HttpContextTool
    {
        private readonly HttpContextBase httpContext;

        public HttpContextTool(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        public string GetAbsolutePath(string virtualPath = null)
        {
            var request = httpContext.Request;
            var originalUrl = virtualPath ?? request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            var host = request.Url != null ? request.Url.Host : null;

            if (!string.IsNullOrEmpty(originalUrl))
            {
                return new Uri(string.Format("http://{0}{1}", host, originalUrl), UriKind.Absolute).AbsolutePath;
            }

            var originalUri = new Uri(string.Format("http://{0}{1}", host, request.RawUrl));
            var path = originalUri.AbsolutePath;

            if (request.PathInfo.Length > 0)
            {
                path = path.Substring(0, path.Length - request.PathInfo.Length);
            }

            return path;
        }
    }
}
