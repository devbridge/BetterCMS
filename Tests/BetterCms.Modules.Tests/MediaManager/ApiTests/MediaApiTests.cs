using System;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Api.DataContracts;
using BetterCms.Module.MediaManager.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.ApiTests
{
    [TestFixture]
    public class MediaApiTests : DatabaseTestBase
    {
        [Test]
        public void Should_Return_Images_Folder_Medias_List_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var medias = CreateFakeMedias(session);
                var repository = CreateRepository(session);

                using (var api = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {                                                            
                    // Images1 folder has 2 images and 2 folders
                    var folder = medias.First( m => m is MediaFolder && m.Title == "Images1");
                    var folderMedias = api.GetFolderMedias(new GetFolderMediasRequest(MediaType.Image, folder.Id));
                    Assert.IsNotNull(folderMedias);
                    Assert.AreEqual(folderMedias.Items.Count, 4);
                    Assert.AreEqual(folderMedias.Items.Count(m => m is MediaImage), 2);
                    Assert.AreEqual(folderMedias.Items.Count(m => m is MediaFolder), 2);
                }
            });
        }

        [Test]
        public void Should_Return_Root_Image_Folder_Medias_List_Successfully()
        {
            RunActionInTransaction(session =>
            {
                CreateFakeMedias(session, false);
                var repository = CreateRepository(session);
                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    // Root images folder has at least 2 folders and at least 3 files
                    var request = new GetFolderMediasRequest(MediaType.Image, itemsCount: 5);

                    var folderMedias = service.GetFolderMedias(request);
                    Assert.IsNotNull(folderMedias);
                    Assert.AreEqual(folderMedias.Items.Count, 5);
                }
            });
        }

        [Test]
        public void Should_Return_Root_Files_Folder_Medias_List_Successfully()
        {
            RunActionInTransaction(session =>
            {
                CreateFakeMedias(session, false);
                var repository = CreateRepository(session);
                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    // Root files folder has at least 1 folder and 1 file
                    var request = new GetFolderMediasRequest(MediaType.File, itemsCount: 2);
                    var folderMedias = service.GetFolderMedias(request);
                    Assert.IsNotNull(folderMedias);
                    Assert.AreEqual(folderMedias.Items.Count, 2);
                }
            });
        }

        [Test]
        public void Should_Return_Images_List_Successfully()
        {
            RunActionInTransaction(session =>
            {
                CreateFakeMedias(session);
                var repository = CreateRepository(session);
                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    var request = new GetImagesRequest(itemsCount: 3);
                    var images = service.GetImages(request);
                    Assert.IsNotNull(images);
                    Assert.AreEqual(images.Items.Count, 3);
                }
            });
        }
        
        [Test]
        public void Should_Return_Ordered_Descending_Images_List_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var medias = CreateFakeMedias(session);
                var repository = CreateRepository(session);
                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    var folder = medias.First(m => m is MediaFolder && m.Title == "Images1");
                    var request = new GetImagesRequest(p => p.Folder.Id == folder.Id, orderDescending: true, order: p => p.Title);
                    request.AddPaging(1, 2);

                    var images = service.GetImages(request);
                    Assert.IsNotNull(images);
                    Assert.AreEqual(images.Items.Count, 1);
                    Assert.AreEqual(images.TotalCount, 2);
                    Assert.AreEqual(images.Items[0].Title, "Image1__1");
                }
            });
        }

        [Test]
        public void Should_Return_Ordered_Images_List_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var medias = CreateFakeMedias(session);
                var repository = CreateRepository(session);
                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    var folder = medias.First(m => m is MediaFolder && m.Title == "Images1");
                    var request = new GetImagesRequest(p => p.Folder.Id == folder.Id, p => p.Title);
                    request.AddPaging(1, 2);

                    var images = service.GetImages(request);
                    Assert.IsNotNull(images);
                    Assert.AreEqual(images.Items.Count, 1);
                    Assert.AreEqual(images.TotalCount, 2);
                    Assert.AreEqual(images.Items[0].Title, "Image1__2");
                }
            });
        }
        
        [Test]
        public void TEST_Should_Return_Ordered_Images_List_Successfully()
        {
            RunActionInTransaction(session =>
            {
                CreateFakeMedias(session);
                var repository = CreateRepository(session);
                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    var request = new GetImagesRequest(order: p => p.Title);
                    var images = service.GetImages(request);
                    Assert.IsNotNull(images);
                    Assert.GreaterOrEqual(images.Items.Count, 0);
                }
            });
        }
        
        [Test]
        public void Should_Return_Files_List_Successfully()
        {
            RunActionInTransaction(session =>
            {                
                CreateFakeMedias(session);
                var repository = CreateRepository(session);

                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    var request = new GetFilesRequest(itemsCount: 3);
                    var files = service.GetFiles(request);
                    Assert.IsNotNull(files);
                    Assert.GreaterOrEqual(files.Items.Count, 1);
                    Assert.LessOrEqual(files.Items.Count, 3);
                }
            });
        }
        
        [Test]
        public void Should_Return_Folders_List_Successfully()
        {
            RunActionInTransaction(session =>
            {
                CreateFakeMedias(session);
                var repository = CreateRepository(session);

                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    var request = new GetFoldersRequest(MediaType.Image, itemsCount: 2);
                    var imageFolders = service.GetFolders(request);
                    Assert.IsNotNull(imageFolders);
                    Assert.LessOrEqual(imageFolders.Items.Count, 2);

                    request = new GetFoldersRequest(MediaType.File, itemsCount: 2);
                    var fileFolders = service.GetFolders(request);
                    Assert.IsNotNull(fileFolders);
                    Assert.LessOrEqual(fileFolders.Items.Count, 2);
                }
            });
        }

        [Test]
        public void Should_Return_File_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var file = CreateFakeFile(session);
                var repository = CreateRepository(session);
                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    var loadedFile = service.GetFile(file.Id);
                    Assert.IsNotNull(loadedFile);
                    Assert.AreEqual(loadedFile.Id, file.Id);
                }
            });
        }
        
        [Test]
        public void Should_Return_Image_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var image = CreateFakeImage(session);
                var repository = CreateRepository(session);
                using (var service = new MediaManagerApiContext(Container.BeginLifetimeScope(), repository))
                {
                    var loadedImage = service.GetImage(image.Id);
                    Assert.IsNotNull(loadedImage);
                    Assert.AreEqual(loadedImage.Id, image.Id);
                }
            });
        }

        private IRepository CreateRepository(ISession session)
        {
            var unitOfWork = new DefaultUnitOfWork(session);
            var repository = new DefaultRepository(unitOfWork);
            return repository;
        }

        private MediaFile CreateFakeFile(ISession session)
        {
            var file = TestDataProvider.CreateNewMediaFile();
            session.SaveOrUpdate(file);
            session.Flush();
            session.Clear();

            return file;
        }

        private MediaImage CreateFakeImage(ISession session)
        {
            var image = TestDataProvider.CreateNewMediaImage();
            session.SaveOrUpdate(image);
            session.Flush();
            session.Clear();

            return image;
        }

        private Media[] CreateFakeMedias(ISession session, bool fakeRoot = true)
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

            MediaFolder rootFolder = null;
            if (fakeRoot)
            {
                rootFolder = TestDataProvider.CreateNewMediaFolder(false);
                rootFolder.Folder = null;
                rootFolder.Title = string.Concat("TestFolder_", Guid.NewGuid().ToString());
            }

            var images1 = TestDataProvider.CreateNewMediaFolder(false);
            var images2 = TestDataProvider.CreateNewMediaFolder(false);
            var images1_1 = TestDataProvider.CreateNewMediaFolder(false);
            var images1_2 = TestDataProvider.CreateNewMediaFolder(false);
            var images2_1 = TestDataProvider.CreateNewMediaFolder(false);
            var images1_1_1 = TestDataProvider.CreateNewMediaFolder(false);
            var images1_1_2 = TestDataProvider.CreateNewMediaFolder(false);

            var files1 = TestDataProvider.CreateNewMediaFolder(false, MediaType.File);
            var files1_1 = TestDataProvider.CreateNewMediaFolder(false, MediaType.File);

            images1.Folder = rootFolder;
            images2.Folder = rootFolder;
            images1_1.Folder = images1;
            images1_2.Folder = images1;
            images2_1.Folder = images2;
            images1_1_1.Folder = images1_1;
            images1_1_2.Folder = images1_1;
            files1.Folder = rootFolder;
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
            var rootFile1 = TestDataProvider.CreateNewMediaFile();
            var file1_1 = TestDataProvider.CreateNewMediaFile(files1);

            rootImage1.Folder = rootFolder;
            rootImage2.Folder = rootFolder;
            rootImage3.Folder = rootFolder;
            rootFile1.Folder = rootFolder;

            rootImage1.Title = "RootImage1";
            rootImage2.Title = "RootImage2";
            rootImage3.Title = "RootImage3";
            image1__1.Title = "Image1__1";
            image1__2.Title = "Image1__2";
            image1_1__1.Title = "Image1_1__1";
            rootFile1.Title = "RootFile1";
            file1_1.Title = "File1_1";

            var medias = new Media[] {
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

            if (!fakeRoot)
            {
                var suffix = Guid.NewGuid().ToString();
                medias.ToList().ForEach(m => m.Title = string.Concat(m.Title, "_", suffix));
            }
            medias.ToList().ForEach(m => session.SaveOrUpdate(m));
            session.Flush();
            session.Clear();

            return medias;
        }
    }
}
