using System;

using BetterCms.Module.Api;
using BetterCms.Module.Api.Operations.Pages.Sitemap;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Sitemaps
{
    [TestFixture]
    public class SitemapsTests : IntegrationTestBase
    {
        [Test]
        public void Should_Get_Sitemaps()
        {
            using (var api = ApiFactory.Create())
            {
                var results = api.Pages.Sitemap.Get(new GetSitemapsRequest());
                Assert.IsNotNull(results);
                Assert.IsNotNull(results.Data);
                Assert.IsNotNull(results.Data.Items);
                Assert.GreaterOrEqual(results.Data.Items.Count, 1);
                Assert.AreNotEqual(results.Data.Items[0].Id, default(Guid));

                var count = results.Data.Items.Count;
                var id = results.Data.Items[0].Id;

                var results2 = api.Pages.SitemapNew.Get(new GetSitemapRequest { SitemapId = results.Data.Items[0].Id });
                Assert.IsNotNull(results2);
                Assert.IsNotNull(results2.Data);
                Assert.AreEqual(results2.Data.Id, id);

                var results3 = api.Pages.Sitemaps.Get(new BetterCms.Module.Api.Operations.Pages.Sitemaps.GetSitemapsRequest());
                Assert.IsNotNull(results3);
                Assert.IsNotNull(results3.Data);
                Assert.IsNotNull(results3.Data.Items);
                Assert.AreEqual(results3.Data.Items.Count, count);
            }
        }
    }
}
