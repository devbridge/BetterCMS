using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class PageOptionMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_PageOption_Successfully()
        {
            var entity = TestDataProvider.CreateNewPageOption();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
