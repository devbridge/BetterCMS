using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Root.Tags;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Root.Tags
{
    public class TagsApiTests : ApiCrudIntegrationTestBase<
        SaveTagModel, TagModel,
        PostTagRequest, PostTagResponse,
        GetTagRequest, GetTagResponse,
        PutTagRequest, PutTagResponse,
        DeleteTagRequest, DeleteTagResponse>
    {
        [Test]
        public void Should_CRUD_Tag_Successfully()
        {
            // Attach to events
            Events.RootEvents.Instance.TagCreated += Instance_EntityCreated;
            Events.RootEvents.Instance.TagUpdated += Instance_EntityUpdated;
            Events.RootEvents.Instance.TagDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Root.Tags.Post, api.Root.Tag.Get, api.Root.Tag.Put, api.Root.Tag.Delete));

            // Detach from events
            Events.RootEvents.Instance.TagCreated -= Instance_EntityCreated;
            Events.RootEvents.Instance.TagUpdated -= Instance_EntityUpdated;
            Events.RootEvents.Instance.TagDeleted -= Instance_EntityDeleted;
        }

        protected override SaveTagModel GetCreateModel(ISession session)
        {
            return new SaveTagModel
                   {
                       Name = TestDataProvider.ProvideRandomString(MaxLength.Name)
                   };
        }

        protected override GetTagRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetTagRequest { TagId = saveResponseBase.Data.Value };
        }

        protected override PutTagRequest GetUpdateRequest(GetTagResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetTagResponse getResponse, SaveTagModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
        }
    }
}
