using System;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class CmsUrlHelper
    {
        public static string GetActionUrl<TController>(System.Linq.Expressions.Expression<System.Action<TController>> urlExpression, bool fullUrl = false)
            where TController : Controller
        {
            var routeValuesFromExpression = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(urlExpression);
            var action = routeValuesFromExpression["Action"].ToString();
            var controller = routeValuesFromExpression["Controller"].ToString();

            var url = new UrlHelper(HttpContext.Current.Request.RequestContext).Action(action, controller, routeValuesFromExpression);
            if (fullUrl)
            {
                url = string.Concat(GetServerUrl().TrimEnd('/'), url);
            }
            url = HttpUtility.UrlDecode(url);

            return url;
        }

        public static string GetFullActionUrl<TController>(System.Linq.Expressions.Expression<System.Action<TController>> urlExpression)
            where TController : Controller
        {
            return GetActionUrl(urlExpression, true);
        }

        public static string GetServerUrl()
        {
            if (string.IsNullOrWhiteSpace(CmsContext.Config.WebSiteUrl) || CmsContext.Config.WebSiteUrl.Equals("auto", StringComparison.InvariantCultureIgnoreCase))
            {
                return HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, null);
            }

            return CmsContext.Config.WebSiteUrl;
        }
    }
}