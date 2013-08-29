using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class PageMapTest : IntegrationTestBase
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
            
            var pageOptions = new[]
                {
                    TestDataProvider.CreateNewPageOption(page),
                    TestDataProvider.CreateNewPageOption(page),
                    TestDataProvider.CreateNewPageOption(page)
                };

            page.Options = pageOptions;

            SaveEntityAndRunAssertionsInTransaction(
                page,
                result =>
                {
                    Assert.AreEqual(page, result);
                    Assert.AreEqual(pageContents.OrderBy(f => f.Id), result.PageContents.OrderBy(f => f.Id));
                });
        }       
    }
}
