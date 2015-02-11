using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

using Devbridge.Platform.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Root.Categories
{
    public class CategoriesApiTests : ApiCrudIntegrationTestBase<
        SaveCategoryModel, CategoryModel,
        PostCategoryRequest, PostCategoryResponse,
        GetCategoryRequest, GetCategoryResponse,
        PutCategoryRequest, PutCategoryResponse,
        DeleteCategoryRequest, DeleteCategoryResponse>
    {
        [Test]
        public void Should_CRUD_Category_Successfully()
        {
            // Attach to events
            Events.RootEvents.Instance.CategoryCreated += Instance_EntityCreated;
            Events.RootEvents.Instance.CategoryUpdated += Instance_EntityUpdated;
            Events.RootEvents.Instance.CategoryDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Root.Categories.Post, api.Root.Category.Get, api.Root.Category.Put, api.Root.Category.Delete));

            // Detach from events
            Events.RootEvents.Instance.CategoryCreated -= Instance_EntityCreated;
            Events.RootEvents.Instance.CategoryUpdated -= Instance_EntityUpdated;
            Events.RootEvents.Instance.CategoryDeleted -= Instance_EntityDeleted;
        }

        protected override SaveCategoryModel GetCreateModel(ISession session)
        {
            return new SaveCategoryModel
                   {
                       Name = TestDataProvider.ProvideRandomString(MaxLength.Name)
                   };
        }

        protected override GetCategoryRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetCategoryRequest { CategoryId = saveResponseBase.Data.Value };
        }

        protected override PutCategoryRequest GetUpdateRequest(GetCategoryResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetCategoryResponse getResponse, SaveCategoryModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
        }
    }
}
