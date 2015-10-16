using System.Web;
using System.Web.Routing;

using BetterCms.Module.Root.Controllers;

namespace BetterCms.Module.Root.Mvc
{
    /// <summary>
    /// Mime types routes constraint.
    /// </summary>
    public class MimeTypeRouteConstraint : IRouteConstraint
    {
        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <param name="route">The object that this constraint belongs to.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        /// <param name="values">An object that contains the parameters for the URL.</param>
        /// <param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
        /// <returns>
        /// true if the URL parameter contains a valid value; otherwise, false.
        /// </returns>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return EmbeddedResourcesController.MimeTypes.ContainsKey(values[parameterName].ToString().ToLowerInvariant());
        }
    }
}