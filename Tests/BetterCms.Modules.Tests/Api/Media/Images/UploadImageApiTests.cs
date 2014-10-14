using System.Reflection;

using BetterCms.Core.Models;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;

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

        [Test]
        public void Should_Upload_Image_From_Stream_Successfully()
        {
            Events.MediaManagerEvents.Instance.MediaFileUploaded += Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileUpdated += Instance_EntityUpdated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Media.Images.Upload.Post, api.Media.Image.Get, api.Media.Image.Delete));

            Events.MediaManagerEvents.Instance.MediaFileUploaded -= Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileUpdated -= Instance_EntityUpdated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted -= Instance_EntityDeleted;
        }
        
        protected override UploadImageModel GetCreateModel(ISession session)
        {
            return new UploadImageModel
            {
                Caption = TestDataProvider.ProvideRandomString(MaxLength.Name),
                Description = TestDataProvider.ProvideRandomString(MaxLength.Text),
                FolderId = null,
                Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                Version = 0,
                FileName = TestBigImageFileName,
                FileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(TestBigImagePath)
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
            Assert.AreEqual(getResponse.Data.OriginalFileName, model.FileName);
            Assert.AreEqual(getResponse.Data.OriginalSize, model.FileStream.Length);
        }
    }
}
