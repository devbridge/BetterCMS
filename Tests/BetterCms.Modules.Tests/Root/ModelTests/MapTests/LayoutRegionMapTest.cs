using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class LayoutRegionMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_LayoutRegion_Successfully()
        {
            var entity = TestDataProvider.CreateNewLayoutRegion();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
