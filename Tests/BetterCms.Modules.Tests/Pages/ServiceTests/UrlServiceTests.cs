using System.Linq;

using BetterCms.Module.Pages.Services;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServiceTests
{
    [TestFixture]
    public class UrlServiceTests : TestBase
    {
        [Test]
        public void Should_Allow_Urls()
        {
            var service = new DefaultUrlService(null, null);

            var validUrls = new[]
                {
                    @"/",
                    @"/a/",
                    @"bsd-asd",
                    @"c/a",
                    @"d/a/a/a/a/a/a/žaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/a",
                    @"e/a/a/a/a/a/a/žaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/aaa/a",
                    @"/fasd/aasd/aasd/aasdasdad/aasdasd/asdasda/žaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/a",
                    @"gsad/aa",
                    @"/hasd",
                    @"/j",
                    @"/ksdasdasd/",
                    @"/شسیللانتلان/",
                    @"/уцкевапрнгш-енгшен/",
                }.ToList();

            validUrls.ForEach(url => Assert.IsTrue(service.ValidateUrl(url), string.Format("URL must be valid: '{0}'", url)));
        }

        [Test]
        public void Should_Deny_Urls()
        {
            var service = new DefaultUrlService(null, null);

            var invalidUrls = new[]
                {
                    @"//",
                    @"a//a",
                    @"b//a/",
                    @"c/a/a/a/a/a/a/žaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa1/a",
                    @"dsdasd\asdasdasdas",
                    @"esdas$asdasdasdasd",
                }.ToList();

            invalidUrls.ForEach(url => Assert.IsFalse(service.ValidateUrl(url), string.Format("URL must be invalid: '{0}'", url)));
        }
    }
}
