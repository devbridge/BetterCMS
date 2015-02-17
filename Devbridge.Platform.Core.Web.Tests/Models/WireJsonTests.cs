using System.Linq;

using Devbridge.Platform.Core.Web.Models;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Models
{
    [TestFixture]
    public class WireJsonTests : TestBase
    {
        [Test]
        public void Should_Initialize_Empty_Constructor()
        {
            var json = new WireJson();

            Assert.IsFalse(json.Success);
            Assert.AreEqual(json.DataType, "html");
            Assert.IsNull(json.Data);
            Assert.IsNull(json.Messages);
        }
        
        [Test]
        public void Should_Initialize_Success()
        {
            var json = new WireJson(true);

            Assert.IsTrue(json.Success);
            Assert.AreEqual(json.DataType, "html");
            Assert.IsNull(json.Data);
            Assert.IsEmpty(json.Messages);
        }
        
        [Test]
        public void Should_Initialize_Success_And_Messages()
        {
            const string message1 = "Message1";
            const string message2 = "Message2";

            var json = new WireJson(true, new[] { message1, message2 });

            Assert.IsTrue(json.Success);
            Assert.AreEqual(json.DataType, "html");
            Assert.IsNull(json.Data);
            Assert.IsNotNull(json.Messages);
            Assert.AreEqual(json.Messages.Length, 2);
            Assert.IsTrue(json.Messages.Any(m => m == message1));
            Assert.IsTrue(json.Messages.Any(m => m == message2));
        }
        
        [Test]
        public void Should_Initialize_Success_Html_And_Messages()
        {
            const string html = "html";
            const string message1 = "Message1";
            const string message2 = "Message2";

            var json = new WireJson(true, html, message1, message2);

            Assert.IsTrue(json.Success);
            Assert.AreEqual(json.DataType, "html");
            Assert.AreEqual(json.Data, html);
            Assert.IsNotNull(json.Messages);
            Assert.AreEqual(json.Messages.Length, 2);
            Assert.IsTrue(json.Messages.Any(m => m == message1));
            Assert.IsTrue(json.Messages.Any(m => m == message2));
        }
        
        [Test]
        public void Should_Initialize_Success_DataType_And_Messages()
        {
            const string data = "html";
            const string dataType = "test-type";
            const string message1 = "Message1";
            const string message2 = "Message2";

            var json = new WireJson(true, dataType, data, new[] { message1, message2 });

            Assert.IsTrue(json.Success);
            Assert.AreEqual(json.DataType, dataType);
            Assert.AreEqual(json.Data, data);
            Assert.IsNotNull(json.Messages);
            Assert.AreEqual(json.Messages.Length, 2);
            Assert.IsTrue(json.Messages.Any(m => m == message1));
            Assert.IsTrue(json.Messages.Any(m => m == message2));
        }

        [Test]
        public void Should_Initialize_Success_And_Object()
        {
            dynamic data = new { Test = 1 };

            var json = new WireJson(true, data);

            Assert.IsTrue(json.Success);
            Assert.IsNull(json.DataType);
            Assert.AreEqual(json.Data, data);
            Assert.IsNull(json.Messages);
        }
    }
}
