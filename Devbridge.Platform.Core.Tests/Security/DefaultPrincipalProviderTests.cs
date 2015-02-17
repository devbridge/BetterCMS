using System.Security.Principal;
using System.Threading;

using Devbridge.Platform.Core.Security;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Security
{
    [TestFixture]
    public class DefaultPrincipalProviderTests : TestBase
    {
        [Test]
        public void Should_Return_CurrectPrincipal()
        {
            var origPrincipal = Thread.CurrentPrincipal;
            var principal = new GenericPrincipal(new GenericIdentity("TestPrincipal1"), null);
            Thread.CurrentPrincipal = principal;

            var principalProvider = new DefaultPrincipalProvider();
            var retrievedPrincipal = principalProvider.GetCurrentPrincipal();

            Assert.AreEqual(principal, retrievedPrincipal);

            Thread.CurrentPrincipal = origPrincipal;
        }
        
        [Test]
        public void Should_Return_CurrectPrincipal_Name()
        {
            var origPrincipal = Thread.CurrentPrincipal;
            var principal = new GenericPrincipal(new GenericIdentity("TestPrincipal2"), null);
            Thread.CurrentPrincipal = principal;

            var principalProvider = new DefaultPrincipalProvider();
            var name = principalProvider.CurrentPrincipalName;

            Assert.AreEqual(name, "TestPrincipal2");

            Thread.CurrentPrincipal = origPrincipal;
        }
        
        [Test]
        public void Should_Return_Anonymous_Principal_Name()
        {
            var origPrincipal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = null;

            var principalProvider = new DefaultPrincipalProvider();
            var name = principalProvider.CurrentPrincipalName;

            Assert.AreEqual(name, DefaultPrincipalProvider.AnonymousPrincipalName);

            Thread.CurrentPrincipal = origPrincipal;
        }
    }
}
