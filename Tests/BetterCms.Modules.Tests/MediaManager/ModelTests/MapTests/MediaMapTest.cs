using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.ModelTests.MapTests
{
    [TestFixture]
    public class MediaMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Media_Successfully()
        {
            var entity = TestDataProvider.CreateNewMedia();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}