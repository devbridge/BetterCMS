using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;

using BetterCms.Events;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

using BetterModules.Core.Models;
using BetterModules.Events;

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

        private readonly List<Guid> sampleGuids =

        // These guids are hardcoded into database migration scripts
        new List<Guid>()
        {
            new Guid("DC861498-FCD1-4F19-9C75-AE71916EF7BF"),
            new Guid("B2F05159-74AF-4B67-AEB9-36B9CC9EED57")
        };

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

        void Instance_CategoryNodeCreated(SingleItemEventArgs<ICategory> args)
        {
            createdNodeEventCount++;
        }

        void Instance_CategoryNodeDeleted(SingleItemEventArgs<ICategory> args)
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
                UseForCategorizableItems = sampleGuids,
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

            Assert.IsNotNull(getResponse.Data.AvailableFor);

            Assert.AreEqual(sampleGuids.Count, getResponse.Data.AvailableFor.Count());

            //            Assert.AreEqual(getResponse.AccessRules[0].AccessLevel, saveModel.AccessRules[0].AccessLevel);
            //            Assert.AreEqual(getResponse.AccessRules[0].Identity, saveModel.AccessRules[0].Identity);
            //            Assert.AreEqual(getResponse.AccessRules[0].IsForRole, saveModel.AccessRules[0].IsForRole);
        }
    }
}