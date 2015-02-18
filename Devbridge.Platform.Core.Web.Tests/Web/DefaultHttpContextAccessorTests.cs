using System;
using System.IO;
using System.Web;

using Devbridge.Platform.Core.Web.Configuration;
using Devbridge.Platform.Core.Web.Web;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Web
{
    [TestFixture]
    public class DefaultHttpContextAccessorTests : TestBase
    {
        private HttpContext currentContext;

        [Test]
        public void Should_Return_Correct_Current_HttpContext()
        {
            CreateHttpContext();
            var webConfiguration = new Mock<IWebConfiguration>();
            var accessor = new DefaultHttpContextAccessor(webConfiguration.Object);
            var current = accessor.GetCurrent();

            Assert.IsNotNull(current);
            Assert.IsTrue(current.Items.Contains("TestKey"));
            Assert.AreEqual(current.Items["TestKey"], "TestValue");

            RestoreContext();
        }

        [Test]
        public void Should_Map_Local_Path_WithoutContext_Correctly()
        {
            currentContext = HttpContext.Current;
            HttpContext.Current = null;

            var webConfiguration = new Mock<IWebConfiguration>();
            var accessor = new DefaultHttpContextAccessor(webConfiguration.Object);

            var path = accessor.MapPath("test\\test1");
            Assert.IsNotNull(path);
            Assert.IsTrue(path.EndsWith("test\\test1"));
            Assert.IsTrue(path.StartsWith(AppDomain.CurrentDomain.BaseDirectory));

            RestoreContext();
        }

        private void CreateHttpContext()
        {
            currentContext = HttpContext.Current;

            var fakeContext = new HttpContext(new HttpRequest("c:\\test.dll", "http://localhost", null), new HttpResponse(new StringWriter()));
            fakeContext.Items.Add("TestKey", "TestValue");
            HttpContext.Current = fakeContext;
        }

        private void RestoreContext()
        {
            HttpContext.Current = currentContext;
        }
    }
}
