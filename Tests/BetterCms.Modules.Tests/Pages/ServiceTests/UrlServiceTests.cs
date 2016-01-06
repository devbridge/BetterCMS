using System.Linq;

using BetterCms.Module.Pages.Services;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServiceTests
{
    [TestFixture]
    public class UrlServiceTests : TestBase
    {
        private static string[] validInternalUrls =
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
                };
        
        private static string[] invalidInternalUrls =
                {
                    @"//",
                    @"a//a",
                    @"b//a/",
                    @"c/a/a/a/a/a/a/žaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa1/a",
                    @"dsdasd\asdasdasdas",
                    @"esdas$asdasdasdasd",
                    @"/<script>",
                    @"src=""test"""
                };

        private static string[] validExternalUrls =
                {
                    @"http://google.com",
                    @"http://www.google.com",
                    @"ssh://google.com/dsfds/dsf/dsfds/dfsf/sdfds/fdsf/sdf/",
                    @"github://google.com/dsfds/dsf/dsfds/dfsf/sdfds/fdsf/sdf/?try=15&a=16#/angular-routing/",
                    @"http://www.google.com?<>?*$#!#@%#^$%(*^&()_+", // after ? mark all symbols are allowed
                    @"http://www.google.com#<>?*$#!#@%#^$%(*^&()_+", // after # mark all symbols are allowed
                };

        private static string[] invalidExternalUrls =
                {
                    @"http://goo<gle.com/?page=1#hash",
                    @"http://goo>gle.com/?page=1#hash",
                    @"http://goo*gle.com/?page=1#hash",
                    @"http://goo&gle.com/?page=1#hash"
                };

        [Test]
        public void Should_Allow_Internal_Urls()
        {
            var service = new DefaultUrlService(null, null);

            validInternalUrls.ToList().ForEach(url => Assert.IsTrue(service.ValidateInternalUrl(url), string.Format("URL must be valid: '{0}'", url)));
        }

        [Test]
        public void Should_Deny_Internal_Urls()
        {
            var service = new DefaultUrlService(null, null);

            invalidInternalUrls.ToList().ForEach(url => Assert.IsFalse(service.ValidateInternalUrl(url), string.Format("URL must be invalid: '{0}'", url)));
        }
        
        [Test]
        public void Should_Allow_External_Urls()
        {
            var service = new DefaultUrlService(null, null);

            validInternalUrls.ToList().ForEach(url => Assert.IsTrue(service.ValidateExternalUrl(url), string.Format("URL must be valid: '{0}'", url)));
            validExternalUrls.ToList().ForEach(url => Assert.IsTrue(service.ValidateExternalUrl(url), string.Format("URL must be valid: '{0}'", url)));
        }

        [Test]
        public void Should_Deny_External_Urls()
        {
            var service = new DefaultUrlService(null, null);

            invalidExternalUrls.ToList().ForEach(url => Assert.IsFalse(service.ValidateExternalUrl(url), string.Format("URL must be invalid: '{0}'", url)));
        }
    }
}
