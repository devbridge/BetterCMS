using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.ModelTests.MapTests
{
    [TestFixture]
    public class MediaFileMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_MediaFile_Successfully()
        {
            var entity = TestDataProvider.CreateNewMediaFile();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}