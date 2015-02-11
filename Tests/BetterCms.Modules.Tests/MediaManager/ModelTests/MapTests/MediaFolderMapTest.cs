using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.ModelTests.MapTests
{
    [TestFixture]
    public class MediaFolderMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_MediaFolder_Successfully()
        {
            var entity = TestDataProvider.CreateNewMediaFolder();
            RunEntityMapTestsInTransaction(entity);
        }

        [Test]
        public void Should_Insert_And_Retrieve_MediaFolder_Files_Successfully()
        {
            var mediaFolder = TestDataProvider.CreateNewMediaFolder();

            var files = new []
                {
                    TestDataProvider.CreateNewMediaFile(mediaFolder),
                    TestDataProvider.CreateNewMediaImage(mediaFolder),
                    TestDataProvider.CreateNewMediaImage( mediaFolder)
                };

            mediaFolder.Medias = files;

            SaveEntityAndRunAssertionsInTransaction(
                mediaFolder,
                result =>
                {
                    Assert.AreEqual(mediaFolder, result);
                    Assert.AreEqual(files.OrderBy(f => f.Id), result.Medias.OrderBy(f => f.Id));
                });
        }
    }
}