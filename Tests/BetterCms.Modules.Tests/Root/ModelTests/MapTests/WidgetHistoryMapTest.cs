using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class WidgetHistoryMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_WidgetHistory_Successfully()
        {
            var entity = TestDataProvider.CreateNewWidgetHistory();
            RunEntityMapTestsInTransaction(entity);
        }

        [Test]
        public void Should_Insert_And_Retrieve_WidgetHistory_ContentOptionHistoryList_Successfully()
        {
            var widget = TestDataProvider.CreateNewWidgetHistory();

            var contentOptions = new[]
                {
                    TestDataProvider.CreateNewContentOptionHistory(widget),
                    TestDataProvider.CreateNewContentOptionHistory(widget)
                };
            widget.ContentOptionHistory = contentOptions;

            SaveEntityAndRunAssertionsInTransaction(
                widget,
                result =>
                {
                    Assert.AreEqual(widget, result);
                    Assert.AreEqual(contentOptions.OrderBy(f => f.Id), result.ContentOptionHistory.OrderBy(f => f.Id));
                });
        }
    }
}
