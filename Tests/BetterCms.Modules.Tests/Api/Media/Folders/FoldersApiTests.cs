using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.MediaManager;
using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;

using BetterModules.Core.Models;
using BetterModules.Events;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Media.Folders
{
    public class FoldersApiTests : ApiCrudIntegrationTestBase<
        SaveFolderModel, FolderModel,
        PostFolderRequest, PostFolderResponse,
        GetFolderRequest, GetFolderResponse,
        PutFolderRequest, PutFolderResponse,
        DeleteFolderRequest, DeleteFolderResponse>
    {
        private int archivedMediaEventCount;

        private int unarchivedMediaEventCount;

        [Test]
        public void Should_CRUD_Folder_Successfully()
        {
            // Attach to events.
            Events.MediaManagerEvents.Instance.MediaFolderCreated += Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFolderUpdated += Instance_EntityUpdated;
            Events.MediaManagerEvents.Instance.MediaFolderDeleted += Instance_EntityDeleted;
            Events.MediaManagerEvents.Instance.MediaArchived += Instance_MediaArchived;
            Events.MediaManagerEvents.Instance.MediaUnarchived += Instance_MediaUnarchived;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Media.Folders.Post, api.Media.Folder.Get, api.Media.Folder.Put, api.Media.Folder.Delete));

            Assert.AreEqual(1, archivedMediaEventCount, "Archived media events fired count");
            Assert.AreEqual(1, unarchivedMediaEventCount, "Unarchived media events fired count");

            // Detach from events.
            Events.MediaManagerEvents.Instance.MediaFolderCreated -= Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFolderUpdated -= Instance_EntityUpdated;
            Events.MediaManagerEvents.Instance.MediaFolderDeleted -= Instance_EntityDeleted;
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

        protected override SaveFolderModel GetCreateModel(ISession session)
        {
            return new SaveFolderModel
                   {
                       Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                       Type = TestDataProvider.ProvideRandomEnumValue<MediaType>(),
                       IsArchived = true,
                       ParentFolderId = null
                   };
        }

        protected override GetFolderRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetFolderRequest { FolderId = saveResponseBase.Data.Value };
        }

        protected override PutFolderRequest GetUpdateRequest(GetFolderResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Title = TestDataProvider.ProvideRandomString(MaxLength.Name);
            request.Data.IsArchived = false;

            return request;
        }

        protected override void OnAfterGet(GetFolderResponse getResponse, SaveFolderModel model)
        {
            Assert.IsNotNull(getResponse.Data.Title);

            Assert.AreEqual(getResponse.Data.Title, model.Title);
            Assert.AreEqual(getResponse.Data.Type, model.Type);
            Assert.AreEqual(getResponse.Data.IsArchived, model.IsArchived);
            Assert.AreEqual(getResponse.Data.ParentFolderId, model.ParentFolderId);
            Assert.AreEqual(getResponse.Data.ParentFolderId, model.ParentFolderId);
        }
    }
}
