using NUnit.Framework;

namespace BetterCms.Test.Module.Sitemap.ModelTests.MapTests
{
    [TestFixture]
    public class SitemapNodeMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_SitemapNode_Successfully()
        {
            var parentNode = TestDataProvider.CreateNewSitemapNode();
//            var childNode1 = TestDataProvider.CreateNewSitemapNode();
//            var childNode2 = TestDataProvider.CreateNewSitemapNode();
//            childNode1.ParentNode = parentNode;
//            childNode2.ParentNode = parentNode;
            RunEntityMapTestsInTransaction(parentNode);
        }
    }
}
