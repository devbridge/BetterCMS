using BetterCms.Core.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.ModelTests.MapTests
{
    [TestFixture]
    public class MediaImageMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_MediaImage_Successfully()
        {
            var entity = TestDataProvider.CreateNewMediaImage();

            RunEntityMapTestsInTransaction(entity);
        }
    }
}