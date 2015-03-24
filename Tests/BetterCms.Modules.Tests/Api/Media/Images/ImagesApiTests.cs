using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;
using BetterModules.Events;

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
        private int archivedMediaEventCount;

        private int unarchivedMediaEventCount;

        private Category category;
        [Test]
        public void Should_CRUD_Image_Successfully()
        {
            Events.MediaManagerEvents.Instance.MediaFileUploaded += Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileUpdated += Instance_EntityUpdated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted += Instance_EntityDeleted;
            Events.MediaManagerEvents.Instance.MediaArchived += Instance_MediaArchived;
            Events.MediaManagerEvents.Instance.MediaUnarchived += Instance_MediaUnarchived;


            RunApiActionInTransaction(
                (api, session) =>
                {
                    category = null;
                    var categoryTree = TestDataProvider.CreateNewCategoryTree();
                    category = TestDataProvider.CreateNewCategory(categoryTree);
                    session.SaveOrUpdate(categoryTree);
                    session.SaveOrUpdate(category);
                    session.Flush();
                    Run(session, api.Media.Images.Post, api.Media.Image.Get, api.Media.Image.Put, api.Media.Image.Delete);

                });

            Assert.AreEqual(1, archivedMediaEventCount, "Archived media events fired count");
            Assert.AreEqual(1, unarchivedMediaEventCount, "Unarchived media events fired count");

            Events.MediaManagerEvents.Instance.MediaFileUploaded -= Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileUpdated -= Instance_EntityUpdated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted -= Instance_EntityDeleted;
            Events.MediaManagerEvents.Instance.MediaArchived -= Instance_MediaArchived;
            Events.MediaManagerEvents.Instance.MediaUnarchived -= Instance_MediaUnarchived;
        }

        void Instance_MediaUnarchived(SingleItemEventArgs<BetterCms.Module.MediaManager.Models.Media> args)
        {
            archivedMediaEventCount++;
        }

        void Instance_MediaArchived(SingleItemEventArgs<BetterCms.Module.MediaManager.Models.Media> args)
        {
            unarchivedMediaEventCount++;
        }

        protected override SaveImageModel GetCreateModel(ISession session)
        {
            return new SaveImageModel
                {
                    Caption = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    Description = TestDataProvider.ProvideRandomString(MaxLength.Text),
                    FileSize = TestDataProvider.ProvideRandomNumber(0, 10000),
                    FileUri = "file:///C:/tmp.jpg",
                    FolderId = null,
                    Height = TestDataProvider.ProvideRandomNumber(1, 1000),
                    ImageUrl = string.Format("{0}/{1}", TestDataProvider.ProvideRandomString(MaxLength.Name), TestDataProvider.ProvideRandomString(MaxLength.Name)),
                    IsArchived = true,
                    IsCanceled = TestDataProvider.ProvideRandomBooleanValue(),
                    IsTemporary = TestDataProvider.ProvideRandomBooleanValue(),
                    IsUploaded = TestDataProvider.ProvideRandomBooleanValue(),
                    OriginalFileExtension = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    OriginalFileName = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    OriginalHeight = TestDataProvider.ProvideRandomNumber(1, 1000),
                    OriginalSize = TestDataProvider.ProvideRandomNumber(0, 10000),
                    OriginalUri = "file:///C:/tmp_orig.jpg",
                    OriginalUrl = TestDataProvider.ProvideRandomString(MaxLength.Url),
                    OriginalWidth = TestDataProvider.ProvideRandomNumber(1, 1000),
                    PublishedOn = TestDataProvider.ProvideRandomDateTime(),
                    Tags = new[] { TestDataProvider.ProvideRandomString(MaxLength.Name), TestDataProvider.ProvideRandomString(MaxLength.Name) },
                    ThumbnailHeight = TestDataProvider.ProvideRandomNumber(1, 100),
                    ThumbnailSize = TestDataProvider.ProvideRandomNumber(1, 100),
                    ThumbnailUri = "file:///C:/tmp_thumbnail.jpg",
                    ThumbnailUrl = TestDataProvider.ProvideRandomString(MaxLength.Url),
                    ThumbnailWidth = TestDataProvider.ProvideRandomNumber(1, 100),
                    Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    Version = 0,
                    Width = TestDataProvider.ProvideRandomNumber(1, 1000),
                    Categories = new List<Guid>() { category.Id }
                };
        }

        protected override GetImageRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetImageRequest { ImageId = saveResponseBase.Data.Value, Data = new GetImageModel() { IncludeTags = true, IncludeCategories = true } };
        }

        protected override PutImageRequest GetUpdateRequest(GetImageResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Title = this.TestDataProvider.ProvideRandomString(MaxLength.Name);
            request.Data.IsArchived = false;
            request.Data.Categories.Clear();
            return request;
        }

        protected override void OnAfterGet(GetImageResponse getResponse, SaveImageModel model)
        {
            Assert.IsNotNull(getResponse.Data.Id);
            Assert.AreEqual(getResponse.Data.Title, model.Title);
            Assert.AreEqual(getResponse.Data.Caption, model.Caption);
            Assert.AreEqual(getResponse.Data.Description, model.Description);
            Assert.AreEqual(getResponse.Data.FileSize, model.FileSize);
            Assert.AreEqual(getResponse.Data.FileUri, model.FileUri);
            Assert.AreEqual(getResponse.Data.FolderId, model.FolderId);
            Assert.AreEqual(getResponse.Data.Height, model.Height);
            Assert.AreEqual(getResponse.Data.ImageUrl, model.ImageUrl);
            Assert.AreEqual(getResponse.Data.IsArchived, model.IsArchived);
            Assert.AreEqual(getResponse.Data.IsCanceled, model.IsCanceled);
            Assert.AreEqual(getResponse.Data.IsTemporary, model.IsTemporary);
            Assert.AreEqual(getResponse.Data.IsUploaded, model.IsUploaded);
            Assert.AreEqual(getResponse.Data.OriginalFileExtension, model.OriginalFileExtension);
            Assert.AreEqual(getResponse.Data.OriginalFileName, model.OriginalFileName);
            Assert.AreEqual(getResponse.Data.OriginalHeight, model.OriginalHeight);
            Assert.AreEqual(getResponse.Data.OriginalSize, model.OriginalSize);
            Assert.AreEqual(getResponse.Data.OriginalUri, model.OriginalUri);
            Assert.AreEqual(getResponse.Data.OriginalUrl, model.OriginalUrl);
            Assert.AreEqual(getResponse.Data.OriginalWidth, model.OriginalWidth);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Tags.Count, model.Tags.Count);
            Assert.AreEqual(getResponse.Data.ThumbnailHeight, model.ThumbnailHeight);
            Assert.AreEqual(getResponse.Data.ThumbnailSize, model.ThumbnailSize);
            Assert.AreEqual(getResponse.Data.ThumbnailUri, model.ThumbnailUri);
            Assert.AreEqual(getResponse.Data.ThumbnailUrl, model.ThumbnailUrl);
            Assert.AreEqual(getResponse.Data.ThumbnailWidth, model.ThumbnailWidth);
            Assert.AreEqual(getResponse.Data.Width, model.Width);

            if (model.Categories != null)
            {
                Assert.AreEqual(model.Categories.Count, getResponse.Data.Categories.Count);

                foreach (var categoryId in model.Categories)
                {
                    Assert.IsNotNull(getResponse.Data.Categories.FirstOrDefault(c => c.Id == categoryId));
                }
            }
        }
    }
}
