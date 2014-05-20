using System;
using System.Collections.Generic;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;

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
        [Ignore]
        [Test]
        public void Should_CRUD_Sitemap_Successfully()
        {
            RunApiActionInTransaction((api, session) => Run(api.Pages.Sitemaps.Post, api.Pages.SitemapNew.Get, api.Pages.SitemapNew.Put, api.Pages.SitemapNew.Delete));
        }

        protected override SaveSitemapModel GetCreateModel()
        {
            return new SaveSitemapModel
                       {
                           Title = "Test Sitemap 01",
                           Tags = new[] { "Test Tag 01", "Test Tag 02" },
                           Nodes =
                               new[]
                                   {
                                       new SaveSitemapNodeModel
                                           {
                                               Title = "Home",
                                               DisplayOrder = 0,
                                               Url = "/",
                                               PageId = null,
                                               Macro = "Some macro text.",
                                               UsePageTitleAsNodeTitle = false,
                                               Translations = new List<SaveSitemapNodeTranslation>(),
                                               Nodes =
                                                   new[]
                                                       {
                                                           new SaveSitemapNodeModel
                                                               {
                                                                   Title = "BetterCMS.com",
                                                                   DisplayOrder = 0,
                                                                   Url = "http://"
                                                               },
                                                       }
                                           },
                                   }
                       };
        }

        protected override GetSitemapRequest GetGetRequest(SaveResponseBase saveResponseBase)
        {
            return new GetSitemapRequest { SitemapId = saveResponseBase.Data.Value };
        }

        protected override PutSitemapRequest GetUpdateRequest(GetSitemapResponse getResponse)
        {
            return getResponse.ToPutRequest();
        }

        protected override void OnAfterGet(GetSitemapResponse getResponse, SaveSitemapModel saveModel)
        {
            base.OnAfterGet(getResponse, saveModel);

            Assert.AreEqual(getResponse.Data.Title, saveModel.Title);
            Assert.AreEqual(getResponse.Data.Tags.Count, saveModel.Tags.Count);
        }
    }
}
