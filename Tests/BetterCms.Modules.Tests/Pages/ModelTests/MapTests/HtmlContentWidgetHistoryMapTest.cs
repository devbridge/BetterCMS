using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class HtmlContentWidgetHistoryMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_HtmlContentWidgetHistory_Successfully()
        {
            var widget = TestDataProvider.CreateNewHtmlContentWidgetHistory();
            RunEntityMapTestsInTransaction(widget);  
        }
    }
}
