using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class ServerControlWidgetMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_ServerControlWidget_Successfully()
        {
            var widget = TestDataProvider.CreateNewServerControlWidget();
            RunEntityMapTestsInTransaction(widget);  
        }
    }
}
