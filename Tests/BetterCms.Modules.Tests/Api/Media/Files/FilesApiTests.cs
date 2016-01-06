using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;
using BetterModules.Events;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Media.Files
{
    public class FilesApiTests : ApiCrudIntegrationTestBase<
        SaveFileModel, FileModel,
        PostFileRequest, PostFileResponse,
        GetFileRequest, GetFileResponse,
        PutFileRequest, PutFileResponse,
        DeleteFileRequest, DeleteFileResponse>
    {
        private int archivedMediaEventCount;

        private int unarchivedMediaEventCount;

        private Category category;
        [Test]
        public void Should_CRUD_File_Successfully()
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

                    Run(session, api.Media.Files.Post, api.Media.File.Get, api.Media.File.Put, api.Media.File.Delete);
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

        protected override SaveFileModel GetCreateModel(ISession session)
        {
            return new SaveFileModel
                {
                    Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    Description = TestDataProvider.ProvideRandomString(MaxLength.Text),
                    FileSize = TestDataProvider.ProvideRandomNumber(0, 10000),
                    FileUri = "file:///C:/tmp.jpg",
                    FolderId = null,
                    IsArchived = true,
                    IsCanceled = TestDataProvider.ProvideRandomBooleanValue(),
                    IsTemporary = TestDataProvider.ProvideRandomBooleanValue(),
                    IsUploaded = TestDataProvider.ProvideRandomBooleanValue(),
                    OriginalFileExtension = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    OriginalFileName = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    PublishedOn = TestDataProvider.ProvideRandomDateTime(),
                    Tags = new[] { TestDataProvider.ProvideRandomString(MaxLength.Name), TestDataProvider.ProvideRandomString(MaxLength.Name) },
                    Categories = new[] { category.Id },
                    ThumbnailId = null,
                    AccessRules = new[]
                                     {           
                                          new AccessRuleModel
                                                {
                                                    AccessLevel = AccessLevel.ReadWrite, 
                                                    Identity = TestDataProvider.ProvideRandomString(20),
                                                    IsForRole = false
                                                },
                                          new AccessRuleModel
                                                {
                                                    AccessLevel = AccessLevel.Deny, 
                                                    Identity = TestDataProvider.ProvideRandomString(20),
                                                    IsForRole = true
                                                }
                                     },
                    PublicUrl = string.Format("{0}/{1}", TestDataProvider.ProvideRandomString(MaxLength.Name), TestDataProvider.ProvideRandomString(MaxLength.Name)),
                    Version = 0,
                };
        }

        protected override GetFileRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetFileRequest { FileId = saveResponseBase.Data.Value, Data = new GetFileModel() { IncludeAccessRules = true, IncludeTags = true , IncludeCategories = true} };
        }

        protected override PutFileRequest GetUpdateRequest(GetFileResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Title = this.TestDataProvider.ProvideRandomString(MaxLength.Name);
            request.Data.IsArchived = false;
            request.Data.Categories.Clear();
            return request;
        }

        protected override void OnAfterGet(GetFileResponse getResponse, SaveFileModel model)
        {
            Assert.IsNotNull(getResponse.Data.Id);
            Assert.AreEqual(getResponse.Data.Title, model.Title);
            Assert.AreEqual(getResponse.Data.Description, model.Description);
            Assert.AreEqual(getResponse.Data.FileSize, model.FileSize);
            Assert.AreEqual(getResponse.Data.FileUri, model.FileUri);
            Assert.AreEqual(getResponse.Data.FolderId, model.FolderId);
            Assert.AreEqual(getResponse.Data.IsArchived, model.IsArchived);
            Assert.AreEqual(getResponse.Data.IsCanceled, model.IsCanceled);
            Assert.AreEqual(getResponse.Data.IsTemporary, model.IsTemporary);
            Assert.AreEqual(getResponse.Data.IsUploaded, model.IsUploaded);
            Assert.AreEqual(getResponse.Data.OriginalFileExtension, model.OriginalFileExtension);
            Assert.AreEqual(getResponse.Data.OriginalFileName, model.OriginalFileName);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Tags.Count, model.Tags.Count);
            Assert.AreEqual(getResponse.Data.ThumbnailId, model.ThumbnailId);
            Assert.AreEqual(getResponse.AccessRules.Count, model.AccessRules.Count);
            Assert.AreEqual(getResponse.Data.FileUrl, model.PublicUrl);

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
