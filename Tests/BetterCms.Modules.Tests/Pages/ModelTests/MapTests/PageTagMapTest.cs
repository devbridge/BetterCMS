using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class PageTagMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_PageTag_Successfully()
        {
            var entity = TestDataProvider.CreateNewPageTag();
            RunEntityMapTestsInTransaction(entity);            
        }
    }
}
