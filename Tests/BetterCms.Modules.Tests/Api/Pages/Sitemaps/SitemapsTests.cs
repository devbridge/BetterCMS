// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapsTests.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;
using BetterCms.Module.Api.Operations.Root;

using BetterModules.Core.Models;
using BetterModules.Events;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Sitemaps
{
    [TestFixture]
    public class SitemapsTests : ApiCrudIntegrationTestBase<
        SaveSitemapModel, SitemapModel,
        PostSitemapRequest, PostSitemapResponse,
        GetSitemapRequest, GetSitemapResponse,
        PutSitemapRequest, PutSitemapResponse,
        DeleteSitemapRequest, DeleteSitemapResponse>
    {
        private int createdNodeEventCount;
        private int deletedNodeEventCount;

        [Test]
        public void Should_CRUD_Sitemap_Successfully()
        {
            // Attach to events
            Events.SitemapEvents.Instance.SitemapCreated += Instance_EntityCreated;
            Events.SitemapEvents.Instance.SitemapUpdated += Instance_EntityUpdated;
            Events.SitemapEvents.Instance.SitemapDeleted += Instance_EntityDeleted;
            Events.SitemapEvents.Instance.SitemapNodeCreated += Instance_SitemapNodeCreated;
            Events.SitemapEvents.Instance.SitemapNodeDeleted += Instance_SitemapNodeDeleted;

            RunApiActionInTransaction((api, session) => Run(session, api.Pages.Sitemaps.Post, api.Pages.SitemapNew.Get, api.Pages.SitemapNew.Put, api.Pages.SitemapNew.Delete));

            Assert.AreEqual(2, createdNodeEventCount, "Created node events fired count");
            Assert.AreEqual(1, deletedNodeEventCount, "Deleted node events fired count");

            // Detach from events
            Events.SitemapEvents.Instance.SitemapCreated -= Instance_EntityCreated;
            Events.SitemapEvents.Instance.SitemapUpdated -= Instance_EntityUpdated;
            Events.SitemapEvents.Instance.SitemapDeleted -= Instance_EntityDeleted;
            Events.SitemapEvents.Instance.SitemapNodeCreated -= Instance_SitemapNodeCreated;
            Events.SitemapEvents.Instance.SitemapNodeDeleted += Instance_SitemapNodeDeleted;
        }

        void Instance_SitemapNodeCreated(SingleItemEventArgs<BetterCms.Module.Pages.Models.SitemapNode> args)
        {
            createdNodeEventCount++;
        }

        void Instance_SitemapNodeDeleted(SingleItemEventArgs<BetterCms.Module.Pages.Models.SitemapNode> args)
        {
            deletedNodeEventCount++;
        }

        protected override SaveSitemapModel GetCreateModel(ISession session)
        {
            return new SaveSitemapModel
                       {
                           Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                           Tags = new[] { TestDataProvider.ProvideRandomString(MaxLength.Name), TestDataProvider.ProvideRandomString(MaxLength.Name) },
                           Nodes =
                               new[]
                                   {
                                       new SaveSitemapNodeModel
                                           {
                                               Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                               DisplayOrder = 0,
                                               Url = TestDataProvider.ProvideRandomString(MaxLength.Url),
                                               PageId = null,
                                               Macro = TestDataProvider.ProvideRandomString(MaxLength.Text),
                                               UsePageTitleAsNodeTitle = false,
                                               Translations = new List<SaveSitemapNodeTranslation>
                                                                  {
                                                                      new SaveSitemapNodeTranslation
                                                                          {
                                                                              Id = Guid.NewGuid(),
                                                                              Title = "Some translation title",
                                                                              Url = TestDataProvider.ProvideRandomString(MaxLength.Url),
                                                                              UsePageTitleAsNodeTitle = false,
                                                                              LanguageId = Guid.Empty,
                                                                              Macro = "test"
                                                                          }
                                                                  },
                                               Nodes =
                                                   new[]
                                                       {
                                                           new SaveSitemapNodeModel
                                                               {
                                                                   Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                                                   DisplayOrder = 0,
                                                                   Url = TestDataProvider.ProvideRandomString(MaxLength.Url),
                                                                   PageId = null,
                                                                   Macro = TestDataProvider.ProvideRandomString(MaxLength.Text),
                                                                   UsePageTitleAsNodeTitle = false,
                                                                   Translations = null,
                                                               },
                                                       }
                                           },
                                   },
                            AccessRules = new[] { new AccessRuleModel { AccessLevel = AccessLevel.ReadWrite, Identity = "Admin", IsForRole = true } }
                       };
        }

        protected override GetSitemapRequest GetGetRequest(SaveResponseBase saveResponseBase)
        {
            return new GetSitemapRequest { SitemapId = saveResponseBase.Data.Value, Data = new GetSitemapModel { IncludeNodes = true, IncludeAccessRules = true, } };
        }

        protected override PutSitemapRequest GetUpdateRequest(GetSitemapResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Title = this.TestDataProvider.ProvideRandomString(MaxLength.Name);
            request.Data.Nodes.First().Translations.First().Url = TestDataProvider.ProvideRandomString(MaxLength.Url);
            request.Data.Nodes.First().Nodes.Clear();
            request.Data.AccessRules.Clear();

            return request;
        }

        protected override void OnAfterGet(GetSitemapResponse getResponse, SaveSitemapModel saveModel)
        {
            base.OnAfterGet(getResponse, saveModel);

            Assert.AreEqual(getResponse.Data.Title, saveModel.Title);
            Assert.AreEqual(getResponse.Data.Tags.Count, saveModel.Tags.Count);

            Assert.AreEqual(getResponse.Nodes.First(n => n.ParentId == null).Title, saveModel.Nodes.First().Title);
            Assert.AreEqual(getResponse.Nodes.First(n => n.ParentId == null).Translations.First().Title, saveModel.Nodes.First().Translations.First().Title);

//            Assert.AreEqual(getResponse.AccessRules[0].AccessLevel, saveModel.AccessRules[0].AccessLevel);
//            Assert.AreEqual(getResponse.AccessRules[0].Identity, saveModel.AccessRules[0].Identity);
//            Assert.AreEqual(getResponse.AccessRules[0].IsForRole, saveModel.AccessRules[0].IsForRole);
        }
    }
}