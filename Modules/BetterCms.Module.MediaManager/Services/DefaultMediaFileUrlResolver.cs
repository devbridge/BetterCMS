using System;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Web;
using BetterCms.Module.MediaManager.Controllers;

namespace BetterCms.Module.MediaManager.Services
{
    public class DefaultMediaFileUrlResolver : IMediaFileUrlResolver
    {
        private readonly IHttpContextAccessor contextAccessor;
        
        private readonly ICmsConfiguration cmsConfiguration;

        public DefaultMediaFileUrlResolver(IHttpContextAccessor contextAccessor, ICmsConfiguration cmsConfiguration)
        {
            this.contextAccessor = contextAccessor;
            this.cmsConfiguration = cmsConfiguration;
        }

        public string GetMediaFileFullUrl(Guid id, string publicUrl)
        {
            var routeValuesFromExpression = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression<FilesController>(f => f.Download(id.ToString()));
            var action = routeValuesFromExpression["Action"].ToString();
            var controller = routeValuesFromExpression["Controller"].ToString();
            var current = contextAccessor.GetCurrent();

            if (current != null)
            {
                string url = new UrlHelper(current.Request.RequestContext).Action(action, controller, routeValuesFromExpression);
                url = string.Concat(GetServerUrl(current.Request).TrimEnd('/'), url);
                url = HttpUtility.UrlDecode(url);

                return url;
            }

            return null;
        }

        private string GetServerUrl(HttpRequestBase request)
        {
            if (request != null && string.IsNullOrWhiteSpace(cmsConfiguration.WebSiteUrl) || cmsConfiguration.WebSiteUrl.Equals("auto", StringComparison.InvariantCultureIgnoreCase))
            {
                return request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, null);
            }

            return cmsConfiguration.WebSiteUrl;
        }
    }
}