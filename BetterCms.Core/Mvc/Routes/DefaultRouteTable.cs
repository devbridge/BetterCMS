using System.Web.Routing;

namespace BetterCms.Core.Mvc.Routes
{
    /// <summary>
    /// Default implementation of <see cref="IRouteTable" /> interface.
    /// </summary>
    public class DefaultRouteTable : IRouteTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRouteTable" /> class.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public DefaultRouteTable(RouteCollection routes)
        {
            Routes = routes;
        }

        /// <inheritdoc/>
        public RouteCollection Routes { get; private set; }
    }
}
