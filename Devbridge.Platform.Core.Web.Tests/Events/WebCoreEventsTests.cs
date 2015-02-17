using System.Web;

using Devbridge.Platform.Events;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Events
{
    [TestFixture]
    public class WebCoreEventsTests : TestBase
    {
        private HttpApplication host;
        private int firedCount;

        [Test]
        public void Should_FireHostStartEvents_Correctly()
        {
            firedCount = 0;
            host = new Mock<HttpApplication>().Object;

            WebCoreEvents.Instance.HostStart += Instance_Event;

            Assert.AreEqual(firedCount, 0);
            WebCoreEvents.Instance.OnHostStart(host);
            Assert.AreEqual(firedCount, 1);
            WebCoreEvents.Instance.OnHostStart(host);
            Assert.AreEqual(firedCount, 2);

            WebCoreEvents.Instance.HostStart -= Instance_Event;
        }
        
        [Test]
        public void Should_FireHostStopEvents_Correctly()
        {
            firedCount = 0;
            host = new Mock<HttpApplication>().Object;

            WebCoreEvents.Instance.HostStop += Instance_Event;

            Assert.AreEqual(firedCount, 0);
            WebCoreEvents.Instance.OnHostStop(host);
            Assert.AreEqual(firedCount, 1);
            WebCoreEvents.Instance.OnHostStop(host);
            Assert.AreEqual(firedCount, 2);

            WebCoreEvents.Instance.HostStop -= Instance_Event;
        }
        
        [Test]
        public void Should_FireHostErrorEvents_Correctly()
        {
            firedCount = 0;
            host = new Mock<HttpApplication>().Object;

            WebCoreEvents.Instance.HostError += Instance_Event;

            Assert.AreEqual(firedCount, 0);
            WebCoreEvents.Instance.OnHostError(host);
            Assert.AreEqual(firedCount, 1);
            WebCoreEvents.Instance.OnHostError(host);
            Assert.AreEqual(firedCount, 2);

            WebCoreEvents.Instance.HostError -= Instance_Event;
        }
        
        [Test]
        public void Should_FireHostAuthenticateRequestEvents_Correctly()
        {
            firedCount = 0;
            host = new Mock<HttpApplication>().Object;

            WebCoreEvents.Instance.HostAuthenticateRequest += Instance_Event;

            Assert.AreEqual(firedCount, 0);
            WebCoreEvents.Instance.OnHostAuthenticateRequest(host);
            Assert.AreEqual(firedCount, 1);
            WebCoreEvents.Instance.OnHostAuthenticateRequest(host);
            Assert.AreEqual(firedCount, 2);

            WebCoreEvents.Instance.HostAuthenticateRequest -= Instance_Event;
        }

        void Instance_Event(SingleItemEventArgs<HttpApplication> args)
        {
            Assert.IsNotNull(args);
            Assert.IsNotNull(args.Item);

            Assert.AreEqual(host, args.Item);

            firedCount++;
        }
    }
}
