using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class PageContentHistoryMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_PageContentHistory_Successfully()
        {
            var entity = TestDataProvider.CreateNewPageContentHistory();
            RunEntityMapTestsInTransaction(entity);            
        }

        [Test]
        public void Should_Insert_And_Retrieve_PageContentHistory_PageContentOptionHistory_Successfully()
        {
            var newPageContentHistory = TestDataProvider.CreateNewPageContentHistory();

            var pageContentOptions = new[]
                {
                    TestDataProvider.CreateNewPageContentOptionHistory(newPageContentHistory),
                    TestDataProvider.CreateNewPageContentOptionHistory(newPageContentHistory)
                };
            newPageContentHistory.Options = pageContentOptions;

            SaveEntityAndRunAssertionsInTransaction(
                newPageContentHistory,
                result =>
                {
                    Assert.AreEqual(newPageContentHistory, result);
                    Assert.AreEqual(pageContentOptions.OrderBy(f => f.Id), result.Options.OrderBy(f => f.Id));
                });
        }
    }
}
