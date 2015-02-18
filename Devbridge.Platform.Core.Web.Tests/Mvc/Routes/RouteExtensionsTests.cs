using System.Web.Routing;

using Devbridge.Platform.Core.Web.Mvc.Routes;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Mvc.Routes
{
    [TestFixture]
    public class RouteExtensionsTests
    {
        [Test]
        public void Should_Resolve_Route_AreaName_Correctly()
        {
            var dictionary = new RouteValueDictionary();
            dictionary["area"] = "TestAreaName";
            var handler = new PageRouteHandler("~/Views");
            var route = new Route("url", null, null, dictionary, handler);
            var routeData = new RouteData(route, handler);

            var areaName = routeData.GetAreaName();
            Assert.AreEqual(areaName, "TestAreaName");
        }
    }
}
