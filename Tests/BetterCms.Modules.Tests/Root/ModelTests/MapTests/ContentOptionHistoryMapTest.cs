using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class ContentOptionHistoryMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_ContentOptionHistory_Successfully()
        {
            var contentOption = TestDataProvider.CreateNewContentOptionHistory();
            RunEntityMapTestsInTransaction(contentOption);            
        }
    }
}
