using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class ContentHistoryMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_ContentHistory_Successfully()
        {
            var contentHistory = TestDataProvider.CreateNewContentHistory();
            RunEntityMapTestsInTransaction(contentHistory);            
        }

        [Test]
        public void Should_Insert_And_Retrieve_ContentHistory_PageContentHistoryList_Successfully()
        {
            var contentHistory = TestDataProvider.CreateNewContentHistory();
            var pageContentHistoryList = new[]
                {
                    TestDataProvider.CreateNewPageContentHistory(contentHistory),
                    TestDataProvider.CreateNewPageContentHistory(contentHistory)
                };
            contentHistory.PageContentHistory = pageContentHistoryList;

            SaveEntityAndRunAssertionsInTransaction(
                contentHistory,
                result =>
                    {
                        Assert.AreEqual(contentHistory, result);
                        Assert.AreEqual(pageContentHistoryList.OrderBy(f => f.Id), result.PageContentHistory.OrderBy(f => f.Id));
                    });
        }

        [Test]
        public void Should_Insert_And_Retrieve_ContentHistory_ContentOptionHistoryList_Successfully()
        {
            var contentHistory = TestDataProvider.CreateNewContentHistory();
            var options = new[]
                {
                    TestDataProvider.CreateNewContentOptionHistory(contentHistory),
                    TestDataProvider.CreateNewContentOptionHistory(contentHistory)
                };
            contentHistory.ContentOptionHistory = options;

            SaveEntityAndRunAssertionsInTransaction(
                contentHistory,
                result =>
                    {
                        Assert.AreEqual(contentHistory, result);
                        Assert.AreEqual(options.OrderBy(f => f.Id), result.ContentOptionHistory.OrderBy(f => f.Id));
                    });
        }
    }
}
