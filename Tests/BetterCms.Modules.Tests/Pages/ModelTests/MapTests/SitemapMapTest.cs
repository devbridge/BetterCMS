using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class SitemapMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Sitemap_Successfully()
        {
            var sitemap = TestDataProvider.CreateNewSitemap();

            RunEntityMapTestsInTransaction(sitemap);
        }
    }
}
