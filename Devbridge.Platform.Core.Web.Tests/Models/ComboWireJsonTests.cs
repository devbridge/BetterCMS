using System.Linq;
using Devbridge.Platform.Core.Web.Models;
using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Models
{
    [TestFixture]
    public class ComboWireJsonTests : TestBase
    {
        [Test]
        public void Should_Initialize_Properties_Correctly()
        {
            dynamic data = new { Test = 1 };
            const string html = "test";
            const string message1 = "Message1";
            const string message2 = "Message2";

            var json = new ComboWireJson(true, html, data, message1, message2);

            Assert.AreEqual(json.Success, true);
            Assert.AreEqual(json.Html, html);
            Assert.AreEqual(json.Data, data);
            Assert.IsNotNull(json.Messages);
            Assert.AreEqual(json.Messages.Length, 2);
            Assert.IsTrue(json.Messages.Any(m => m == message1));
            Assert.IsTrue(json.Messages.Any(m => m == message2));
        }
    }
}
