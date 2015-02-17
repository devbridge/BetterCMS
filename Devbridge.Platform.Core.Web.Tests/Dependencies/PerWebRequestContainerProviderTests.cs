using System.Collections.Generic;

using Devbridge.Platform.Core.Web.Dependencies;
using Devbridge.Platform.Core.Web.Tests.TestHelpers;
using Devbridge.Platform.Core.Web.Web;

using Moq;

using NHibernate.Util;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Dependencies
{
    [TestFixture]
    public class PerWebRequestContainerProviderTests : TestBase
    {
        [Test]
        public void Should_Register_Scope_In_The_Context_Container()
        {
            var contextMock = new HttpContextMoq();
            var htttContextAccessor = new Mock<IHttpContextAccessor>();
            htttContextAccessor
                .Setup(m => m.GetCurrent())
                .Returns(() => contextMock.MockContext.Object);

            var dictionary = new Dictionary<object, object>();
            contextMock.MockContext.Setup(m => m.Items).Returns(dictionary);

            var provider = new PerWebRequestContainerProvider(htttContextAccessor.Object);

            var scope1 = provider.CurrentScope;
            Assert.IsNotNull(scope1);
            Assert.AreEqual(dictionary.Count, 1);
            Assert.AreEqual(((KeyValuePair<object, object>)dictionary.First()).Value, scope1);

            var scope2 = provider.CurrentScope;
            Assert.AreEqual(scope1, scope2);
            Assert.AreEqual(dictionary.Count, 1);
            Assert.AreEqual(((KeyValuePair<object, object>)dictionary.First()).Value, scope2);
        }
    }
}
