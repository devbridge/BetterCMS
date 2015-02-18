using System.Security.Principal;
using System.Threading;

using Devbridge.Platform.Core.Security;
using Devbridge.Platform.Core.Web.Security;
using Devbridge.Platform.Core.Web.Tests.TestHelpers;
using Devbridge.Platform.Core.Web.Web;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Security
{
    [TestFixture]
    public class DefaultWebPrincipalProviderTests : TestBase
    {
        [Test]
        public void Should_Return_Current_Principal()
        {
            var accessor = new Mock<IHttpContextAccessor>();
            var contextMock = new HttpContextMoq();
            var fakePrincipal = new GenericPrincipal(new GenericIdentity("TEST"), null);
            contextMock.MockContext.Setup(r => r.User).Returns(() => fakePrincipal);
            accessor.Setup(r => r.GetCurrent()).Returns(() => contextMock.MockContext.Object);

            var provider = new DefaultWebPrincipalProvider(accessor.Object);
            var principal = provider.GetCurrentPrincipal();

            Assert.AreEqual(principal, fakePrincipal);
        }

        [Test]
        public void Should_Return_Base_Principal()
        {
            var currentPrincipal = Thread.CurrentPrincipal;
            var fakePrincipal = new GenericPrincipal(new GenericIdentity("TEST"), null);
            Thread.CurrentPrincipal = fakePrincipal;

            var accessor = new Mock<IHttpContextAccessor>();
            var provider = new DefaultWebPrincipalProvider(accessor.Object);

            var principal = provider.GetCurrentPrincipal();

            Assert.IsNotNull(principal);
            Assert.AreEqual(principal, fakePrincipal);

            Thread.CurrentPrincipal = currentPrincipal;
        }
    }
}
