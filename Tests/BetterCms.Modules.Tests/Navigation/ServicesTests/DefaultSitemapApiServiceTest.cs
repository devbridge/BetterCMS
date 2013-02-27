using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Navigation.DataServices;
using BetterCms.Module.Navigation.Models;
using BetterCms.Module.Navigation.Services;
using BetterCms.Module.Pages.DataServices;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

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

            var service = new DefaultSitemapApiService(repositoryMock.Object, serviceMock.Object);

            var tree = service.GetSitemapTree();
            Assert.IsNotNull(tree);
            Assert.AreEqual(sitemap.Count(), tree.Count);
            
            var node = service.GetNode(new Guid());
            Assert.IsNotNull(node);

// TODO: need to mockup extension method.
//            var nodes = service.GetNodes();
//            Assert.IsNotNull(nodes);
//            Assert.AreEqual(sitemap.Count(), nodes.Count);
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
// TODO: need to mockup extension method.
//            mock.Setup(r => r.AsQueryable())
//                .Returns(sitemap[0]);

            return mock;
        }
    }
}
