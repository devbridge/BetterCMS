// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodesTests.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;

using BetterModules.Core.Models;
using BetterModules.Events;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Sitemaps.Nodes
{
    [TestFixture]
    public class NodesTests : ApiCrudIntegrationTestBase<
        SaveNodeModel, NodeModel,
        PostSitemapNodeRequest, PostSitemapNodeResponse,
        GetNodeRequest, GetNodeResponse,
        PutNodeRequest, PutNodeResponse,
        DeleteNodeRequest, DeleteNodeResponse>
    {
        private int updatedSitemapEventCount;

        private Guid SitemapId { get; set; }

        [Test]
        public void Should_CRUD_Node_Successfully()
        {
            // Attach to events
            Events.SitemapEvents.Instance.SitemapUpdated += Instance_SitemapUpdated;
            Events.SitemapEvents.Instance.SitemapNodeCreated += Instance_EntityCreated;
            Events.SitemapEvents.Instance.SitemapNodeUpdated += Instance_EntityUpdated;
            Events.SitemapEvents.Instance.SitemapNodeDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction(
                (api, session) =>
                    {
                        var sitemap = this.TestDataProvider.CreateNewSitemap();
                        session.SaveOrUpdate(sitemap);
                        session.Flush();
                        SitemapId = sitemap.Id;

                        Run(session, api.Pages.SitemapNew.Nodes.Post, api.Pages.SitemapNew.Node.Get, api.Pages.SitemapNew.Node.Put, api.Pages.SitemapNew.Node.Delete);
                    });

            Assert.AreEqual(3, updatedSitemapEventCount, "Updated sitemap events fired count");

            // Detach from events
            Events.SitemapEvents.Instance.SitemapNodeCreated -= Instance_EntityCreated;
            Events.SitemapEvents.Instance.SitemapNodeUpdated -= Instance_EntityUpdated;
            Events.SitemapEvents.Instance.SitemapNodeDeleted -= Instance_EntityDeleted;
        }

        void Instance_SitemapUpdated(SingleItemEventArgs<BetterCms.Module.Pages.Models.Sitemap> args)
        {
            updatedSitemapEventCount++;
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
                           Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                           Url = TestDataProvider.ProvideRandomString(MaxLength.Url),
                           PageId = null,
                           DisplayOrder = 1,
                           Macro = TestDataProvider.ProvideRandomString(MaxLength.Text),
                           Translations = new[]
                                              {
                                                  new SaveNodeTranslation
                                                      {
                                                          LanguageId = languageA.Id,
                                                          Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                                          Macro = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                                          Url = TestDataProvider.ProvideRandomString(MaxLength.Url),
                                                          UsePageTitleAsNodeTitle = TestDataProvider.ProvideRandomBooleanValue()
                                                      },
                                                  new SaveNodeTranslation
                                                      {
                                                          LanguageId = languageB.Id,
                                                          Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                                          Macro = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                                          Url = TestDataProvider.ProvideRandomString(MaxLength.Url),
                                                          UsePageTitleAsNodeTitle = TestDataProvider.ProvideRandomBooleanValue()
                                                      },
                                              },
                           UsePageTitleAsNodeTitle = false,
                           ParentId = null
                       };
        }

        protected override GetNodeRequest GetGetRequest(SaveResponseBase saveResponseBase)
        {
            return new GetNodeRequest
                       {
                           SitemapId = this.SitemapId,
                           NodeId = saveResponseBase.Data.Value,
                           Data = new GetNodeModel() { IncludeTranslations = true }
                       };
        }

        protected override PostSitemapNodeRequest GetCreateRequest(SaveNodeModel model)
        {
            var request = base.GetCreateRequest(model);
            request.SitemapId = this.SitemapId;
            return request;
        }

        protected override PutNodeRequest GetUpdateRequest(GetNodeResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Title = this.TestDataProvider.ProvideRandomString(MaxLength.Name);
            return request;
        }

        protected override DeleteNodeRequest GetDeleteRequest(GetNodeResponse getResponse)
        {
            var request = base.GetDeleteRequest(getResponse);
            request.SitemapId = getResponse.Data.SitemapId;
            return request;
        }

        protected override void OnAfterGet(GetNodeResponse getResponse, SaveNodeModel saveModel)
        {
            base.OnAfterGet(getResponse, saveModel);

            Assert.AreEqual(getResponse.Data.Title, saveModel.Title);
            Assert.AreEqual(getResponse.Data.Url, saveModel.Url);
            Assert.AreEqual(getResponse.Data.DisplayOrder, saveModel.DisplayOrder);
            Assert.AreEqual(getResponse.Data.Macro, saveModel.Macro);
            Assert.AreEqual(getResponse.Data.UsePageTitleAsNodeTitle, saveModel.UsePageTitleAsNodeTitle);
            Assert.AreEqual(getResponse.Data.PageId, saveModel.PageId);
            Assert.AreEqual(getResponse.Data.ParentId, saveModel.ParentId);

            Assert.IsNotNull(getResponse.Translations);
            Assert.AreEqual(getResponse.Translations.Count, saveModel.Translations.Count);
        }
    }
}