using System.Web.Routing;

using Devbridge.Platform.Core.Web.Mvc.Routes;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Mvc.Routes
{
    [TestFixture]
    public class DefaultRouteTableTests
    {
        [Test]
        public void Should_Initialize_RouteTable_Properties_Correctly()
        {
            var routesCollection = new RouteCollection();
            var table = new DefaultRouteTable(routesCollection);

            Assert.AreEqual(table.Routes, routesCollection);
        }
    }
}
