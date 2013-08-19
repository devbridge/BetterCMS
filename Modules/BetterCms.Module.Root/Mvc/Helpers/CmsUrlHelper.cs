using System.Web;
using System.Web.Mvc;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class CmsUrlHelper
    {
        public static string GetActionUrl<TController>(System.Linq.Expressions.Expression<System.Action<TController>> urlExpression)
            where TController : Controller
        {
            var routeValuesFromExpression = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(urlExpression);
            var action = routeValuesFromExpression["Action"].ToString();
            var controller = routeValuesFromExpression["Controller"].ToString();

            var url = new UrlHelper(HttpContext.Current.Request.RequestContext).Action(action, controller, routeValuesFromExpression);
            url = HttpUtility.UrlDecode(url);

            return url;
        }
    }
}