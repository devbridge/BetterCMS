using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Api.Operations.Blog.Authors.Author;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Blog.Authors
{
    public class AuthorsApiTests : ApiCrudIntegrationTestBase<
        SaveAuthorModel, AuthorModel,
        PostAuthorRequest, PostAuthorResponse,
        GetAuthorRequest, GetAuthorResponse,
        PutAuthorRequest, PutAuthorResponse,
        DeleteAuthorRequest, DeleteAuthorResponse>
    {
        [Test]
        public void Should_CRUD_Author_Successfully()
        {
            // Attach to events
            Events.BlogEvents.Instance.AuthorCreated += Instance_EntityCreated;
            Events.BlogEvents.Instance.AuthorUpdated += Instance_EntityUpdated;
            Events.BlogEvents.Instance.AuthorDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Blog.Authors.Post, api.Blog.Author.Get, api.Blog.Author.Put, api.Blog.Author.Delete));

            // Detach from events
            Events.BlogEvents.Instance.AuthorCreated -= Instance_EntityCreated;
            Events.BlogEvents.Instance.AuthorUpdated -= Instance_EntityUpdated;
            Events.BlogEvents.Instance.AuthorDeleted -= Instance_EntityDeleted;
        }

        protected override SaveAuthorModel GetCreateModel(ISession session)
        {
            var image = TestDataProvider.CreateNewMediaImage();
            session.SaveOrUpdate(image);

            return new SaveAuthorModel
                {
                    Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    Description = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    ImageId = image.Id
                };
        }

        protected override GetAuthorRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetAuthorRequest { AuthorId = saveResponseBase.Data.Value };
        }

        protected override PutAuthorRequest GetUpdateRequest(GetAuthorResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);
            request.Data.Description = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetAuthorResponse getResponse, SaveAuthorModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.ImageId);
            Assert.IsNotNull(getResponse.Data.Description);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.ImageId, model.ImageId);
            Assert.AreEqual(getResponse.Data.Description, model.Description);
        }
    }
}
