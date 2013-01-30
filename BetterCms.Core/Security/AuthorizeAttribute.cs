using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterCms.Core.Security
{
    /// <summary>
    /// Authorization attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class BcmsAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
// NOTE: brain storming.
//        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
//        {
//            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole("BCMS_SUPER_USER_ROLE"))
//            {
//                return true;
//            }
//
//            return base.AuthorizeCore(httpContext);
//        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
