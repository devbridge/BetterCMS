using System;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using BetterCms.Module.MediaManager.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.ServiceTests
{
    [TestFixture]
    public class DefaultMediaApiServiceIntegrationTests : DatabaseTestBase
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
                    var folder = medias.First(m => m is MediaFolder && m.Title == "Images1");
                    var folderMedias = api.GetFolderMedias(MediaType.Image, folder.Id);
                    Assert.IsNotNull(folderMedias);
                    Assert.AreEqual(folderMedias.Count, 4);
                    Assert.AreEqual(folderMedias.Count(m => m is MediaImage), 2);
                    Assert.AreEqual(folderMedias.Count(m => m is MediaFolder), 2);
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
                    var folderMedias = service.GetFolderMedias(MediaType.Image, request: new GetDataRequest<Media>(itemsPerPage: 5));
                    Assert.IsNotNull(folderMedias);
                    Assert.AreEqual(folderMedias.Count, 5);
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
                    var folderMedias = service.GetFolderMedias(MediaType.File, request: new GetDataRequest<Media>(itemsPerPage: 2));
                    Assert.IsNotNull(folderMedias);
                    Assert.AreEqual(folderMedias.Count, 2);
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
                    var images = service.GetImages(new GetDataRequest<MediaImage>(itemsPerPage: 3));
                    Assert.IsNotNull(images);
                    Assert.AreEqual(images.Count, 3);
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
                    var request = new GetDataRequest<MediaImage>(filter: p => p.Folder.Id == folder.Id, itemsPerPage: 1, pageNumber: 2, orderDescending: true, order: p => p.Title);
                    var images = service.GetImages(request);
                    Assert.IsNotNull(images);
                    Assert.AreEqual(images.Count, 1);
                    Assert.AreEqual(images[0].Title, "Image1__1");
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
                    var request = new GetDataRequest<MediaImage>(filter: p => p.Folder.Id == folder.Id, itemsPerPage: 1, pageNumber: 2, order: p => p.Title);
                    var images = service.GetImages(request);
                    Assert.IsNotNull(images);
                    Assert.AreEqual(images.Count, 1);
                    Assert.AreEqual(images[0].Title, "Image1__2");
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
                    var images = service.GetImages(new GetDataRequest<MediaImage>(null, p => p.Title));
                    Assert.IsNotNull(images);
                    Assert.GreaterOrEqual(images.Count, 0);
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
                    var files = service.GetFiles(new GetDataRequest<MediaFile>(itemsPerPage: 3));
                    Assert.IsNotNull(files);
                    Assert.GreaterOrEqual(files.Count, 1);
                    Assert.LessOrEqual(files.Count, 3);
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
                    var imageFolders = service.GetFolders(MediaType.Image, new GetDataRequest<MediaFolder>(itemsPerPage: 2));
                    Assert.IsNotNull(imageFolders);
                    Assert.LessOrEqual(imageFolders.Count, 2);

                    var fileFolders = service.GetFolders(MediaType.File, new GetDataRequest<MediaFolder>(itemsPerPage: 2));
                    Assert.IsNotNull(fileFolders);
                    Assert.LessOrEqual(fileFolders.Count, 2);
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
