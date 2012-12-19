using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class HtmlContentWidgetMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_HtmlContentWidget_Successfully()
        {
            var widget = TestDataProvider.CreateNewHtmlContentWidget();
            RunEntityMapTestsInTransaction(widget);  
        }
    }
}
