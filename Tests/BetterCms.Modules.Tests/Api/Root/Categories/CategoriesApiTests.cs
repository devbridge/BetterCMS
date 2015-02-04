using System.Linq;

using BetterCms.Core.Models;
using BetterCms.Events;
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Root.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Root.Categories
{
    [TestFixture]
    public class CategoriesApiTests : ApiCrudIntegrationTestBase<
        SaveCategoryTreeModel, CategoryTreeModel,
        PostCategoryTreeRequest, PostCategoryTreeResponse,
        GetCategoryTreeRequest, GetCategoryTreeResponse,
        PutCategoryTreeRequest, PutCategoryTreeResponse,
        DeleteCategoryTreeRequest, DeleteCategoryTreeResponse>
    {
        private int createdNodeEventCount;
        private int deletedNodeEventCount;

        [Test]
        public void Should_CRUD_CategoryTree_Successfully()
        {
            // Attach to events
            RootEvents.Instance.CategoryTreeCreated += Instance_EntityCreated;
            RootEvents.Instance.CategoryTreeUpdated += Instance_EntityUpdated;
            RootEvents.Instance.CategoryTreeDeleted += Instance_EntityDeleted;
            RootEvents.Instance.CategoryCreated += Instance_CategoryNodeCreated;
            RootEvents.Instance.CategoryDeleted += Instance_CategoryNodeDeleted;

            RunApiActionInTransaction((api, session) => Run(session, api.Root.Categories.Post, api.Root.Category.Get, api.Root.Category.Put, api.Root.Category.Delete));

            Assert.AreEqual(2, createdNodeEventCount, "Created node events fired count");
            Assert.AreEqual(1, deletedNodeEventCount, "Deleted node events fired count");

            // Detach from events
            RootEvents.Instance.CategoryTreeCreated -= Instance_EntityCreated;
            RootEvents.Instance.CategoryTreeUpdated -= Instance_EntityUpdated;
            RootEvents.Instance.CategoryTreeDeleted -= Instance_EntityDeleted;
            RootEvents.Instance.CategoryCreated -= Instance_CategoryNodeCreated;
            RootEvents.Instance.CategoryDeleted += Instance_CategoryNodeDeleted;
        }

        void Instance_CategoryNodeCreated(SingleItemEventArgs<Category> args)
        {
            createdNodeEventCount++;
        }

        void Instance_CategoryNodeDeleted(SingleItemEventArgs<Category> args)
        {
            deletedNodeEventCount++;
        }

//        protected override void CheckCreateEvent()
//        {
//            CheckEventsCount(0, 0, 0);
//        }
//
//        protected override void CheckDeleteEvent()
//        {
//            CheckEventsCount(0, 0, 0);
//        }

        protected override SaveCategoryTreeModel GetCreateModel(ISession session)
        {
            return new SaveCategoryTreeModel
            {
                Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                Nodes =
                    new[]
                                   {
                                       new SaveCategoryTreeNodeModel
                                           {
                                               Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                               DisplayOrder = 0,
                                               Macro = TestDataProvider.ProvideRandomString(MaxLength.Text),
                                               Nodes =
                                                   new[]
                                                       {
                                                           new SaveCategoryTreeNodeModel
                                                               {
                                                                   Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                                                   DisplayOrder = 0,
                                                                   Macro = TestDataProvider.ProvideRandomString(MaxLength.Text)
                                                               },
                                                       }
                                           },
                                   },
//                AccessRules = new[] { new AccessRuleModel { AccessLevel = AccessLevel.ReadWrite, Identity = "Admin", IsForRole = true } }
            };
        }

        protected override GetCategoryTreeRequest GetGetRequest(SaveResponseBase saveResponseBase)
        {
            return new GetCategoryTreeRequest { CategoryTreeId = saveResponseBase.Data.Value, Data = new GetCategoryTreeModel { IncludeNodes = true/*, IncludeAccessRules = true,*/ } };
        }

        protected override PutCategoryTreeRequest GetUpdateRequest(GetCategoryTreeResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = this.TestDataProvider.ProvideRandomString(MaxLength.Name);
            request.Data.Nodes.First().Nodes.Clear();
//            request.Data.AccessRules.Clear();

            return request;
        }

        protected override void OnAfterGet(GetCategoryTreeResponse getResponse, SaveCategoryTreeModel saveModel)
        {
            base.OnAfterGet(getResponse, saveModel);

            Assert.AreEqual(getResponse.Data.Name, saveModel.Name);

            Assert.AreEqual(getResponse.Nodes.First(n => n.ParentId == null).Name, saveModel.Nodes.First().Name);

            //            Assert.AreEqual(getResponse.AccessRules[0].AccessLevel, saveModel.AccessRules[0].AccessLevel);
            //            Assert.AreEqual(getResponse.AccessRules[0].Identity, saveModel.AccessRules[0].Identity);
            //            Assert.AreEqual(getResponse.AccessRules[0].IsForRole, saveModel.AccessRules[0].IsForRole);
        }
    }
}