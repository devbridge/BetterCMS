using System;
using Microsoft.AspNet.Http;

namespace BetterCms.Core.Web
{
    /// <summary>
    /// TODO: Move to helpers
    /// </summary>
    public class HttpContextTool
    {
        private readonly HttpContext httpContext;

        public HttpContextTool(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        public string GetAbsolutePath(string virtualPath = null)
        {
            var request = httpContext.Request;
            var originalUrl = virtualPath ?? request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            var host = request.Host;

            if (!string.IsNullOrEmpty(originalUrl))
            {
                return new Uri($"http://{host}{originalUrl}", UriKind.Absolute).AbsolutePath;
            }

            var originalUri = new Uri($"http://{host}{request.RawUrl}");
            var path = originalUri.AbsolutePath;

            if (request.PathInfo.Length > 0)
            {
                path = path.Substring(0, path.Length - request.PathInfo.Length);
            }

            return path;
        }
    }
}
