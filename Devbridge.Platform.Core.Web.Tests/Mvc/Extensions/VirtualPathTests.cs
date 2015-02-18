using Devbridge.Platform.Core.Web.Mvc.Extensions;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Mvc.Extensions
{
    [TestFixture]
    public class VirtualPathTests
    {
        [Test]
        public void Should_Combine_Path_Correctly()
        {
            var path = VirtualPath.Combine("c:\\a", "b", "f");

            Assert.AreEqual(path, "c:/a/b/f");
        }
        
        [Test]
        public void Should_Resolve_Local_Path_Correctly()
        {
            Assert.IsTrue(VirtualPath.IsLocalPath("local"));
            Assert.IsTrue(VirtualPath.IsLocalPath("(local)"));
            Assert.IsFalse(VirtualPath.IsLocalPath("http://www.google.com"));
            Assert.IsFalse(VirtualPath.IsLocalPath("other"));
        }
    }
}
