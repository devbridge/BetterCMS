using BetterCms.Core.Models;
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Media.Images
{
    public class ImagesApiTests : ApiCrudIntegrationTestBase<
        SaveImageModel, ImageModel,
        PostImageRequest, PostImageResponse,
        GetImageRequest, GetImageResponse,
        PutImageRequest, PutImageResponse,
        DeleteImageRequest, DeleteImageResponse>
    {
        [Test]
        public void Should_CRUD_Image_Successfully()
        {
            Events.MediaManagerEvents.Instance.MediaFileUploaded += Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileUpdated += Instance_EntityUpdated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Media.Images.Post, api.Media.Image.Get, api.Media.Image.Put, api.Media.Image.Delete));

            Events.MediaManagerEvents.Instance.MediaFileUploaded -= Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileUpdated -= Instance_EntityUpdated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted -= Instance_EntityDeleted;
        }

        protected override SaveImageModel GetCreateModel(ISession session)
        {
            return new SaveImageModel
                {
                    Caption = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    Description = TestDataProvider.ProvideRandomString(MaxLength.Text),
                    FileSize = TestDataProvider.ProvideRandomNumber(0, 10000),
                    FileUri = "C:/tmp.jpg",
                    FolderId = null,
                    Height = TestDataProvider.ProvideRandomNumber(1, 1000),
                    ImageUrl = TestDataProvider.ProvideRandomString(MaxLength.Url),
                    IsArchived = TestDataProvider.ProvideRandomBooleanValue(),
                    IsCanceled = TestDataProvider.ProvideRandomBooleanValue(),
                    IsTemporary = TestDataProvider.ProvideRandomBooleanValue(),
                    IsUploaded = TestDataProvider.ProvideRandomBooleanValue(),
                    OriginalFileExtension = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    OriginalFileName = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    OriginalHeight = TestDataProvider.ProvideRandomNumber(1, 1000),
                    OriginalSize = TestDataProvider.ProvideRandomNumber(0, 10000),
                    OriginalUri = "C:/tmp_orig.jpg",
                    OriginalUrl = TestDataProvider.ProvideRandomString(MaxLength.Url),
                    OriginalWidth = TestDataProvider.ProvideRandomNumber(1, 1000),
                    PublishedOn = TestDataProvider.ProvideRandomDateTime(),
                    Tags = new[] { TestDataProvider.ProvideRandomString(MaxLength.Name), TestDataProvider.ProvideRandomString(MaxLength.Name) },
                    ThumbnailHeight = TestDataProvider.ProvideRandomNumber(1, 100),
                    ThumbnailSize = TestDataProvider.ProvideRandomNumber(1, 100),
                    ThumbnailUri = "C:/tmp_thumbnail.jpg",
                    ThumbnailUrl = TestDataProvider.ProvideRandomString(MaxLength.Url),
                    ThumbnailWidth = TestDataProvider.ProvideRandomNumber(1, 100),
                    Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    Version = 0,
                    Width = TestDataProvider.ProvideRandomNumber(1, 1000)
                };
        }

        protected override GetImageRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetImageRequest { ImageId = saveResponseBase.Data.Value };
        }

        protected override PutImageRequest GetUpdateRequest(GetImageResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Title = this.TestDataProvider.ProvideRandomString(MaxLength.Name);
            return request;
        }

        protected override void OnAfterGet(GetImageResponse getResponse, SaveImageModel model)
        {
            Assert.IsNotNull(getResponse.Data.Title);
            Assert.IsNotNull(getResponse.Data.Id);

            Assert.AreEqual(getResponse.Data.Title, model.Title);
        }
    }
}
