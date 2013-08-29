using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class PageContentOptionMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_PageContentOption_Successfully()
        {
            var entity = TestDataProvider.CreateNewPageContentOption();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
