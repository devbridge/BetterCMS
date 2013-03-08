using System;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Navigation.ServicesTests
{
    [TestFixture]
    public class DefaultSitemapApiServiceTest : TestBase
    {
        [Test]
        public void Should_Return_History_List_Successfully()
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
                Assert.AreEqual(sitemap.Count(), nodes.Count);
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

        private static Mock<IRepository> MockRepository(SitemapNode[] sitemap)
        {
            var mock = new Mock<IRepository>();

            mock.Setup(r => r.First<SitemapNode>(It.IsAny<Guid>()))
                .Returns(sitemap[0]);

            mock.Setup(r => r.AsQueryable<SitemapNode>())
                .Returns(sitemap.AsQueryable());

            return mock;
        }
    }
}
