using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class ServerControlWidgetHistoryMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_ServerControlWidgetHistory_Successfully()
        {
            var widget = TestDataProvider.CreateNewServerControlWidgetHistory();
            RunEntityMapTestsInTransaction(widget);  
        }
    }
}
