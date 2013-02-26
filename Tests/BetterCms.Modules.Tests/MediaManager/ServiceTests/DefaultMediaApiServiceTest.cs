using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.MediaManager.DataServices;
using BetterCms.Module.MediaManager.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.ServiceTests
{
    [TestFixture]
    public class DefaultMediaApiServiceTest : TestBase
    {
        [Test]
        public void Should_Return_Images_Folder_Medias_List_Successfully()
        {
            var medias = CreateFakeMedias();
            var repository = MockRepository(medias);
            var service = new DefaultMediaApiService(repository.Object);

            // Images1 folder has 2 images and 2 folders
            var folder = medias.First(m => m is MediaFolder && m.Title == "Images1");
            var folderMedias = service.GetFolderMedias(MediaType.Image, folder.Id);
            Assert.IsNotNull(folderMedias);
            Assert.AreEqual(folderMedias.Count, 4);
            Assert.AreEqual(folderMedias.Count(m => m is MediaImage), 2);
            Assert.AreEqual(folderMedias.Count(m => m is MediaFolder), 2);
        }

        [Test]
        public void Should_Return_Root_Image_Folder_Medias_List_Successfully()
        {
            var medias = CreateFakeMedias();
            var repository = MockRepository(medias);
            var service = new DefaultMediaApiService(repository.Object);

            // Root images folder has 2 folders and 3 files
            var folderMedias = service.GetFolderMedias(MediaType.Image);
            Assert.IsNotNull(folderMedias);
            Assert.AreEqual(folderMedias.Count, 5);
            Assert.AreEqual(folderMedias.Count(m => m is MediaImage), 3);
            Assert.AreEqual(folderMedias.Count(m => m is MediaFolder), 2);
        }

        [Test]
        public void Should_Return_Root_Files_Folder_Medias_List_Successfully()
        {
            var medias = CreateFakeMedias();
            var repository = MockRepository(medias);
            var service = new DefaultMediaApiService(repository.Object);

            // Root files folder has 1 folder and 1 file
            var folderMedias = service.GetFolderMedias(MediaType.File);
            Assert.IsNotNull(folderMedias);
            Assert.AreEqual(folderMedias.Count, 2);
            Assert.AreEqual(folderMedias.Count(m => m is MediaImage), 1);
            Assert.AreEqual(folderMedias.Count(m => m is MediaFolder), 1);
        }

        private Mock<IRepository> MockRepository(Media[] medias)
        {
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<Media>())
                .Returns(medias.AsQueryable());

            return repositoryMock;
        }

        private Media[] CreateFakeMedias()
        {
            // Files and folders structure:
            // Images1
            //      Images1_1
            //          Images1_1_1
            //          Images1_1_2
            //          FILE:Image1_1__1
            //      Images1_2
            //      FILE:Image1__1
            //      FILE:Image1__2
            // Images2
            //      Images2_1
            // Files1
            //      Files1_1
            //      FILE:File1_1
            // FILE:RootImage1
            // FILE:RootImage2
            // FILE:RootImage3
            // FILE:RootFile1

            var images1 = TestDataProvider.CreateNewMediaFolder(false);
            var images2 = TestDataProvider.CreateNewMediaFolder(false);
            var images1_1 = TestDataProvider.CreateNewMediaFolder(false);
            var images1_2 = TestDataProvider.CreateNewMediaFolder(false);
            var images2_1 = TestDataProvider.CreateNewMediaFolder(false);
            var images1_1_1 = TestDataProvider.CreateNewMediaFolder(false);
            var images1_1_2 = TestDataProvider.CreateNewMediaFolder(false);

            var files1 = TestDataProvider.CreateNewMediaFolder(false, MediaType.File);
            var files1_1 = TestDataProvider.CreateNewMediaFolder(false, MediaType.File);

            images1.Folder = null;
            images2.Folder = null;
            images1_1.Folder = images1;
            images1_2.Folder = images1;
            images2_1.Folder = images2;
            images1_1_1.Folder = images1_1;
            images1_1_2.Folder = images1_1;
            files1.Folder = null;
            files1_1.Folder = files1;

            images1.Title = "Images1";
            images2.Title = "Images2";
            images1_1.Title = "Images1_1";
            images1_2.Title = "Images1_2";
            images2_1.Title = "Images2_1";
            images1_1_1.Title = "Images1_1_1";
            images1_1_2.Title = "Images1_1_2";
            files1.Title = "Files1";
            files1_1.Title = "Files1_1";

            var rootImage1 = TestDataProvider.CreateNewMediaImage();
            var rootImage2 = TestDataProvider.CreateNewMediaImage();
            var rootImage3 = TestDataProvider.CreateNewMediaImage();
            var image1__1 = TestDataProvider.CreateNewMediaImage(images1);
            var image1__2 = TestDataProvider.CreateNewMediaImage(images1);
            var image1_1__1 = TestDataProvider.CreateNewMediaImage(images1_1);
            var rootFile1 = TestDataProvider.CreateNewMediaImage(null, MediaType.File);
            var file1_1 = TestDataProvider.CreateNewMediaImage(files1, MediaType.File);

            rootImage1.Folder = null;
            rootImage2.Folder = null;
            rootImage3.Folder = null;
            rootFile1.Folder = null;

            rootImage1.Title = "RootImage1";
            rootImage2.Title = "RootImage2";
            rootImage3.Title = "RootImage3";
            image1__1.Title = "Image1__1";
            image1__2.Title = "Image1__2";
            image1_1__1.Title = "Image1_1__1";
            rootFile1.Title = "RootFile1";
            file1_1.Title = "File1_1";

            return new Media[] {
                           // Image folders
                           images1,
                           images2,
                           images1_1,
                           images1_2,
                           images2_1,
                           images1_1_1,
                           images1_1_2,

                           // File folders
                           files1,
                           files1_1,

                           // Images
                           rootImage1,
                           rootImage2,
                           rootImage3,
                           image1__1,
                           image1__2,
                           image1_1__1,

                           // Files
                           rootFile1,
                           file1_1
                       };
        }
    }
}
