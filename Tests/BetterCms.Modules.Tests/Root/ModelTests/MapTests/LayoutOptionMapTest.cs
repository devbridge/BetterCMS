using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class LayoutOptionMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_LayoutOption_Successfully()
        {
            var layoutOption = TestDataProvider.CreateNewLayoutOption();
            RunEntityMapTestsInTransaction(layoutOption);            
        }
    }
}
