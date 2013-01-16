using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class PageMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Page_Successfully()
        {            
            var entity = TestDataProvider.CreateNewPage();
            RunEntityMapTestsInTransaction(entity);
        }

        [Test]
        public void Should_Insert_And_Retrieve_Page_PageContents_Successfully()
        {
            var page = TestDataProvider.CreateNewPage();

            var pageContents = new[]
                {
                    TestDataProvider.CreateNewPageContent(null, page),
                    TestDataProvider.CreateNewPageContent(null, page),
                    TestDataProvider.CreateNewPageContent(null, page)
                };

            page.PageContents = pageContents;

            SaveEntityAndRunAssertionsInTransaction(
                page,
                result =>
                {
                    Assert.AreEqual(page, result);
                    Assert.AreEqual(pageContents.OrderBy(f => f.Id), result.PageContents.OrderBy(f => f.Id));
                });
        }

        [Test]
        public void Should_Insert_And_Retrieve_Page_PageContentHistory_Successfully()
        {
            var page = TestDataProvider.CreateNewPage();

            var pageContentHistory = new[]
                {
                    TestDataProvider.CreateNewPageContentHistory(null, page),
                    TestDataProvider.CreateNewPageContentHistory(null, page),
                    TestDataProvider.CreateNewPageContentHistory(null, page)
                };

            page.PageContentHistory = pageContentHistory;

            SaveEntityAndRunAssertionsInTransaction(
                page,
                result =>
                {
                    Assert.AreEqual(page, result);
                    Assert.AreEqual(pageContentHistory.OrderBy(f => f.Id), result.PageContentHistory.OrderBy(f => f.Id));
                });
        }
    }
}
