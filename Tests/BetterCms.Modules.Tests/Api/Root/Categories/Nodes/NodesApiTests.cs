// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodesApiTests.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;

using BetterCms.Core.DataContracts;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;

using BetterModules.Core.Models;
using BetterModules.Events;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Root.Categories.Nodes
{
    [TestFixture]
    public class NodesApiTests : ApiCrudIntegrationTestBase<
        SaveNodeModel, NodeModel,
        PostCategoryNodeRequest, PostCategoryNodeResponse,
        GetNodeRequest, GetNodeResponse,
        PutNodeRequest, PutNodeResponse,
        DeleteNodeRequest, DeleteNodeResponse>
    {
        private int updatedCategoryTreeEventCount;

        private Guid CategoryTreeId { get; set; }

        [Test]
        public void Should_CRUD_Node_Successfully()
        {
            // Attach to events
            Events.RootEvents.Instance.CategoryTreeUpdated += Instance_SitemapUpdated;
            Events.RootEvents.Instance.CategoryCreated += Instance_EntityCreated;
            Events.RootEvents.Instance.CategoryUpdated += Instance_EntityUpdated;
            Events.RootEvents.Instance.CategoryDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction(
                (api, session) =>
                    {
                        var categoryTree = this.TestDataProvider.CreateNewCategoryTree();
                        session.SaveOrUpdate(categoryTree);
                        session.Flush();
                        CategoryTreeId = categoryTree.Id;

                        Run(session, api.Root.Category.Nodes.Post, api.Root.Category.Node.Get, api.Root.Category.Node.Put, api.Root.Category.Node.Delete);
                    });

            Assert.AreEqual(3, updatedCategoryTreeEventCount, "Updated category tree events fired count");

            // Detach from events
            Events.RootEvents.Instance.CategoryCreated -= Instance_EntityCreated;
            Events.RootEvents.Instance.CategoryUpdated -= Instance_EntityUpdated;
            Events.RootEvents.Instance.CategoryDeleted -= Instance_EntityDeleted;
        }

        void Instance_SitemapUpdated(SingleItemEventArgs<ICategoryTree> args)
        {
            updatedCategoryTreeEventCount++;
        }

        protected override SaveNodeModel GetCreateModel(ISession session)
        {
            var languageA = TestDataProvider.CreateNewLanguage();
            var languageB = TestDataProvider.CreateNewLanguage();
            session.SaveOrUpdate(languageA);
            session.SaveOrUpdate(languageB);
            session.Flush();
            session.Clear();

            return new SaveNodeModel
                       {
                           Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                           DisplayOrder = 1,
                           Macro = TestDataProvider.ProvideRandomString(MaxLength.Text),
                           ParentId = null
                       };
        }

        protected override GetNodeRequest GetGetRequest(SaveResponseBase saveResponseBase)
        {
            return new GetNodeRequest
                       {
                           CategoryTreeId = this.CategoryTreeId,
                           NodeId = saveResponseBase.Data.Value
                       };
        }

        protected override PostCategoryNodeRequest GetCreateRequest(SaveNodeModel model)
        {
            var request = base.GetCreateRequest(model);
            request.CategoryTreeId = this.CategoryTreeId;
            return request;
        }

        protected override PutNodeRequest GetUpdateRequest(GetNodeResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = this.TestDataProvider.ProvideRandomString(MaxLength.Name);
            return request;
        }

        protected override DeleteNodeRequest GetDeleteRequest(GetNodeResponse getResponse)
        {
            var request = base.GetDeleteRequest(getResponse);
            request.CategoryTreeId = getResponse.Data.CategoryTreeId;
            return request;
        }

        protected override void OnAfterGet(GetNodeResponse getResponse, SaveNodeModel saveModel)
        {
            base.OnAfterGet(getResponse, saveModel);

            Assert.AreEqual(getResponse.Data.Name, saveModel.Name);
            Assert.AreEqual(getResponse.Data.DisplayOrder, saveModel.DisplayOrder);
            Assert.AreEqual(getResponse.Data.Macro, saveModel.Macro);
            Assert.AreEqual(getResponse.Data.ParentId, saveModel.ParentId);
        }
    }
}