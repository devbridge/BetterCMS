using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class PageContentOptionHistoryMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_PageContentOptionHistory_Successfully()
        {
            var entity = TestDataProvider.CreateNewPageContentOptionHistory();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
