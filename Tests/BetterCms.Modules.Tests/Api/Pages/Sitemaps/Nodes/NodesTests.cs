using System;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Sitemaps.Nodes
{
    [TestFixture]
    public class NodesTests : ApiCrudIntegrationTestBase<
        SaveNodeModel, SitemapNodeModel,
        PostSitemapNodeRequest, PostSitemapNodeResponse,
        GetNodeRequest, GetNodeResponse,
        PutNodeRequest, PutNodeResponse,
        DeleteNodeRequest, DeleteNodeResponse>
    {
        private Guid SitemapId { get; set; }

        [Test]
        public void Should_CRUD_Node_Successfully()
        {
            this.RunApiActionInTransaction(
                (api, session) =>
                    {
                        var sitemap = this.TestDataProvider.CreateNewSitemap();
                        session.SaveOrUpdate(sitemap);
                        session.Flush();
                        this.SitemapId = sitemap.Id;

                        this.Run(session, api.Pages.SitemapNew.Nodes.Post, api.Pages.SitemapNew.Node.Get, api.Pages.SitemapNew.Node.Put, api.Pages.SitemapNew.Node.Delete);
                    });
        }

        protected override SaveNodeModel GetCreateModel(ISession session)
        {
            return new SaveNodeModel
                       {
                           Title = "Test Node 01",
                           Url = "/test-page-url/",
                           PageId = null,
                           DisplayOrder = 1,
                           Macro = "Macro field test",
                           Translations = new SaveNodeTranslation[0],
                           UsePageTitleAsNodeTitle = false,
                           ParentId = null
                       };
        }

        protected override GetNodeRequest GetGetRequest(SaveResponseBase saveResponseBase)
        {
            return new GetNodeRequest
                       {
                           SitemapId = this.SitemapId,
                           NodeId = saveResponseBase.Data.Value
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
            return getResponse.ToPutRequest();
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
        }
    }
}