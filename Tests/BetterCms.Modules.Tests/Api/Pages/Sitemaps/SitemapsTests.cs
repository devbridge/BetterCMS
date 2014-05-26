using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Models;
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;
using BetterCms.Module.Api.Operations.Root;

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

        [Test]
        public void Should_CRUD_Sitemap_Successfully()
        {
            // Attach to events
            Events.SitemapEvents.Instance.SitemapCreated += Instance_EntityCreated;
            Events.SitemapEvents.Instance.SitemapUpdated += Instance_EntityUpdated;
            Events.SitemapEvents.Instance.SitemapDeleted += Instance_EntityDeleted;
            Events.SitemapEvents.Instance.SitemapNodeCreated += Instance_SitemapNodeCreated;

            RunApiActionInTransaction((api, session) => Run(session, api.Pages.Sitemaps.Post, api.Pages.SitemapNew.Get, api.Pages.SitemapNew.Put, api.Pages.SitemapNew.Delete));

            Assert.AreEqual(2, createdNodeEventCount, "Created node events fired count");

            // Detach from events
            Events.SitemapEvents.Instance.SitemapCreated -= Instance_EntityCreated;
            Events.SitemapEvents.Instance.SitemapUpdated -= Instance_EntityUpdated;
            Events.SitemapEvents.Instance.SitemapDeleted -= Instance_EntityDeleted;
            Events.SitemapEvents.Instance.SitemapNodeCreated -= Instance_SitemapNodeCreated;
        }

        void Instance_SitemapNodeCreated(Events.SingleItemEventArgs<BetterCms.Module.Pages.Models.SitemapNode> args)
        {
            createdNodeEventCount++;
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
                                               Translations = new List<SaveSitemapNodeTranslation>(),
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
            return request;
        }

        protected override void OnAfterGet(GetSitemapResponse getResponse, SaveSitemapModel saveModel)
        {
            base.OnAfterGet(getResponse, saveModel);

            Assert.AreEqual(getResponse.Data.Title, saveModel.Title);
            Assert.AreEqual(getResponse.Data.Tags.Count, saveModel.Tags.Count);

            Assert.AreEqual(getResponse.Nodes.Count, 2);
            Assert.AreEqual(getResponse.Nodes.First(n => n.ParentId == null).Title, saveModel.Nodes.First().Title);

            Assert.AreEqual(getResponse.AccessRules.Count, 1);
            Assert.AreEqual(getResponse.AccessRules[0].AccessLevel, saveModel.AccessRules[0].AccessLevel);
            Assert.AreEqual(getResponse.AccessRules[0].Identity, saveModel.AccessRules[0].Identity);
            Assert.AreEqual(getResponse.AccessRules[0].IsForRole, saveModel.AccessRules[0].IsForRole);
        }
    }
}