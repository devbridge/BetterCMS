using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class SitemapNodeMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_SitemapNode_Successfully()
        {
            var sitemap = TestDataProvider.CreateNewSitemap();

            var parentNode = TestDataProvider.CreateNewSitemapNode(sitemap);
            var childNode1 = TestDataProvider.CreateNewSitemapNode(sitemap);
            var childNode2 = TestDataProvider.CreateNewSitemapNode(sitemap);
            childNode1.ParentNode = parentNode;
            childNode2.ParentNode = parentNode;

            RunEntityMapTestsInTransaction(parentNode);
        }
    }
}
