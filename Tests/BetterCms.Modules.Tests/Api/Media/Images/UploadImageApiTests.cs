using System.Reflection;

using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.MediaManager.Models;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Media.Images
{
    public class UploadImageApiTests : ApiCrdIntegrationTestBase<
        UploadImageModel, ImageModel,
        UploadImageRequest, UploadImageResponse,
        GetImageRequest, GetImageResponse,
        DeleteImageRequest, DeleteImageResponse>
    {
        protected const string TestBigImageFileName = "logo.big.png";
        protected const string TestBigImagePath = "BetterCms.Test.Module.Contents.Images.logo.big.png";

        private MediaFolder folder;

        [Test]
        public void Should_Upload_Image_From_Stream_Successfully()
        {
            Events.MediaManagerEvents.Instance.MediaFileUploaded += Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                {
                    folder = TestDataProvider.CreateNewMediaFolder(false);
                    session.SaveOrUpdate(folder);
                    session.Flush();

                    Run(session, api.Media.Images.Upload.Post, api.Media.Image.Get, api.Media.Image.Delete);
                });

            Events.MediaManagerEvents.Instance.MediaFileUploaded -= Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted -= Instance_EntityDeleted;
        }

        [Test]
        [ExpectedException(typeof(CmsApiValidationException))]
        public void Should_Throw_Exception_For_Invalid_Folder_Type()
        {
            RunApiActionInTransaction((api, session) =>
            {
                folder = TestDataProvider.CreateNewMediaFolder(false, MediaType.File);
                session.SaveOrUpdate(folder);
                session.Flush();

                Run(session, api.Media.Images.Upload.Post, api.Media.Image.Get, api.Media.Image.Delete);
            });
        }

        protected override UploadImageModel GetCreateModel(ISession session)
        {
            return new UploadImageModel
            {
                Caption = TestDataProvider.ProvideRandomString(MaxLength.Name),
                Description = TestDataProvider.ProvideRandomString(MaxLength.Text),
                FolderId = folder.Id,
                Title = TestDataProvider.ProvideRandomString(50),
                Version = 0,
                FileName = TestBigImageFileName,
                FileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(TestBigImagePath),
                WaitForUploadResult = true
            };
        }

        protected override GetImageRequest GetGetRequest(SaveResponseBase saveResponseBase)
        {
            return new GetImageRequest { ImageId = saveResponseBase.Data.Value, Data = new GetImageModel() };
        }

        protected override void OnAfterGet(GetImageResponse getResponse, UploadImageModel model)
        {
            Assert.IsNotNull(getResponse.Data.Id);
            Assert.AreEqual(getResponse.Data.Title, model.Title);
            Assert.AreEqual(getResponse.Data.Caption, model.Caption);
            Assert.AreEqual(getResponse.Data.Description, model.Description);
            Assert.AreEqual(getResponse.Data.FolderId, model.FolderId);
            Assert.AreEqual(getResponse.Data.FolderName, folder.Title);
            Assert.AreEqual(getResponse.Data.OriginalFileName, model.FileName);
            Assert.AreEqual(getResponse.Data.OriginalSize, model.FileStream.Length);
            Assert.AreEqual(getResponse.Data.FileSize, model.FileStream.Length);
            
            Assert.AreEqual(getResponse.Data.FileExtension, ".png");
            Assert.AreEqual(getResponse.Data.OriginalFileExtension, ".png");

            Assert.AreEqual(getResponse.Data.OriginalWidth, 2000);
            Assert.AreEqual(getResponse.Data.OriginalHeight, 554);
            Assert.AreEqual(getResponse.Data.Height, 554);
            Assert.AreEqual(getResponse.Data.Width, 2000);
            Assert.AreEqual(getResponse.Data.ThumbnailWidth, 150);
            Assert.AreEqual(getResponse.Data.ThumbnailHeight, 150);
            Assert.Greater(getResponse.Data.ThumbnailSize, 0);
            Assert.AreEqual(getResponse.Data.IsArchived, false);
            Assert.AreEqual(getResponse.Data.IsTemporary, false);
            Assert.AreEqual(getResponse.Data.IsCanceled, false);
            Assert.IsNotNull(getResponse.Data.PublishedOn);
            Assert.AreEqual(getResponse.Data.IsUploaded, true);

            const string urlStart = "http://bettercms.sandbox.mvc4.local.net/uploads/image/";

            Assert.IsTrue(getResponse.Data.OriginalUrl.StartsWith(urlStart)
                && getResponse.Data.OriginalUrl.EndsWith(string.Format("/o_{0}", TestBigImageFileName)));
            Assert.IsTrue(getResponse.Data.ThumbnailUrl.StartsWith(urlStart)
               && getResponse.Data.ThumbnailUrl.EndsWith(string.Format("/t_{0}", TestBigImageFileName)));
            Assert.IsTrue(getResponse.Data.ImageUrl.StartsWith(urlStart)
               && getResponse.Data.ImageUrl.EndsWith(string.Format("/{0}", TestBigImageFileName)));

            Assert.IsTrue(getResponse.Data.OriginalUri.EndsWith(string.Format("/o_{0}", TestBigImageFileName)));
            Assert.IsTrue(getResponse.Data.ThumbnailUri.EndsWith(string.Format("/t_{0}", TestBigImageFileName)));
            Assert.IsTrue(getResponse.Data.FileUri.EndsWith(string.Format("/{0}", TestBigImageFileName)));
        }
    }
}
