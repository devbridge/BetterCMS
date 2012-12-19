using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class ContentOptionMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_ContentOption_Successfully()
        {
            var contentOption = TestDataProvider.CreateNewContentOption();
            RunEntityMapTestsInTransaction(contentOption);            
        }
    }
}
