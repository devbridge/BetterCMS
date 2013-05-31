using System;
using System.Linq;

using BetterCms.Api;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ApiTests
{
    [TestFixture]
    public class SitemapApiTests : ApiTestBase
    {
        [Test]
        public void Should_Return_Sitemap_List_Successfully()
        {
            var sitemap = CreateFakeSitemap();
            var repositoryMock = MockRepository(sitemap);
            var serviceMock = MockSitemapService(sitemap);

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object, serviceMock.Object))
            {
                var tree = service.GetSitemapTree();
                Assert.IsNotNull(tree);
                Assert.AreEqual(sitemap.Count(), tree.Count);

                var node = service.GetNode(new Guid());
                Assert.IsNotNull(node);

                var nodes = service.GetNodes();
                Assert.IsNotNull(nodes);
                Assert.AreEqual(sitemap.Count(), nodes.Items.Count);
            }
        }

        private SitemapNode[] CreateFakeSitemap()
        {
            var node = TestDataProvider.CreateNewSitemapNode();

            return new[] { node };
        }

        private static Mock<ISitemapService> MockSitemapService(SitemapNode[] sitemap)
        {
            var serviceMock = new Mock<ISitemapService>();
            serviceMock
                .Setup(h => h.GetRootNodes(It.IsAny<string>()))
                .Returns(sitemap ?? new SitemapNode[0]);

            return serviceMock;
        }
    }
}
