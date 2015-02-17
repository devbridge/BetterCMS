using System.Web.Routing;

using Devbridge.Platform.Core.Web.Modules.Registration;
using Devbridge.Platform.Sample.Web.Module;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Modules.Registration
{
    [TestFixture]
    public class WebModuleRegistrationContextTests : TestBase
    {
        [Test]
        public void Should_Initialize_Context_Correctly()
        {
            var descriptor = new SampleWebModuleDescriptor();
            var context = new WebModuleRegistrationContext(descriptor);

            Assert.AreEqual(context.ModuleDescriptor, descriptor);
            Assert.IsNotNull(context.GetRegistrationName());
        }

        [Test]
        public void Should_Ignore_Routes_Correctly()
        {
            var descriptor = new SampleWebModuleDescriptor();
            var context = new WebModuleRegistrationContext(descriptor);

            context.IgnoreRoute("url1");
            context.IgnoreRoute("url2", new { Constraint = "Constraint" });

            Assert.AreEqual(context.Routes.Count, 2);

            var route1 = (Route)context.Routes[0];
            Assert.AreEqual(route1.Url, "url1");
            Assert.IsEmpty(route1.Constraints);
            Assert.IsTrue(route1.RouteHandler is StopRoutingHandler);

            var route2 = (Route)context.Routes[1];
            Assert.AreEqual(route2.Url, "url2");
            Assert.IsNotEmpty(route2.Constraints);
            Assert.IsTrue(route2.RouteHandler is StopRoutingHandler);
        }

        [Test]
        public void Should_Map_Routes_Correctly()
        {
            var descriptor = new SampleWebModuleDescriptor();
            var context = new WebModuleRegistrationContext(descriptor);

            var route1 = context.MapRoute("n1", "url1");
            var route2 = context.MapRoute("n2", "url2", new [] {"ns1", "ns2"});
            var route3 = context.MapRoute("n3", "url3", new { Default = "Default" });
            var route4 = context.MapRoute("n4", "url4", new { Default = "Default" }, new { Constraint = "Constraint" });
            var route5 = context.MapRoute("n5", "url5", new { Default = "Default" }, new { Constraint = "Constraint" }, new [] {"ns3", "ns4"});
            var route6 = context.MapRoute("n6", "url6", new { Default = "Default" }, new[] { "ns5", "ns6" });

            Assert.AreEqual(context.Routes.Count, 6);

            var i = 1;
            foreach (var route in new[]  { route1, route2, route3, route4, route5, route6 })
            {
                Assert.IsNotNull(route);
                Assert.AreEqual(route.Url, string.Concat("url", i));
                Assert.AreEqual(route1.DataTokens["area"], descriptor.AreaName);
                
                Assert.IsTrue(context.Routes.Contains(route));

                i++;
            }

            // Defaults
            Assert.IsEmpty(route1.Defaults);
            Assert.IsEmpty(route2.Defaults);
            Assert.IsNotEmpty(route3.Defaults);
            Assert.IsNotEmpty(route4.Defaults);
            Assert.IsNotEmpty(route5.Defaults);
            Assert.IsNotEmpty(route6.Defaults);
            
            // Defaults
            Assert.IsEmpty(route1.Constraints);
            Assert.IsEmpty(route2.Constraints);
            Assert.IsEmpty(route3.Constraints);
            Assert.IsNotEmpty(route4.Constraints);
            Assert.IsNotEmpty(route5.Constraints);
            Assert.IsEmpty(route6.Constraints);

            // Namespaces
            Assert.IsNull(route1.DataTokens["Namespaces"]);
            Assert.IsNotNull(route2.DataTokens["Namespaces"]);
            Assert.IsNull(route3.DataTokens["Namespaces"]);
            Assert.IsNull(route4.DataTokens["Namespaces"]);
            Assert.IsNotNull(route5.DataTokens["Namespaces"]);
            Assert.IsNotNull(route6.DataTokens["Namespaces"]);

            // Namespace fallback
            Assert.AreEqual(route1.DataTokens["UseNamespaceFallback"], true);
            Assert.AreEqual(route2.DataTokens["UseNamespaceFallback"], false);
            Assert.AreEqual(route3.DataTokens["UseNamespaceFallback"], true);
            Assert.AreEqual(route4.DataTokens["UseNamespaceFallback"], true);
            Assert.AreEqual(route5.DataTokens["UseNamespaceFallback"], false);
            Assert.AreEqual(route6.DataTokens["UseNamespaceFallback"], false);
        }
    }
}
