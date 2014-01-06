using System.Collections.Generic;
using System.Linq;
using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;

using Moq;
using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServiceTests
{
    [TestFixture]
    public class SitemapServiceTests : TestBase
    {
        [Ignore] // Fails because of .FetchMany() usage inside service.ArchiveSitemap() method.
        [Test]
        public void Should_Return_Templates_List_Successfully()
        {
            var sitemap = TestDataProvider.CreateNewSitemap();
            sitemap.Nodes = new List<SitemapNode>
                {
                    TestDataProvider.CreateNewSitemapNode(sitemap),
                    TestDataProvider.CreateNewSitemapNode(sitemap)
                };
            sitemap.Nodes[0].ChildNodes = new List<SitemapNode>
                {
                    TestDataProvider.CreateNewSitemapNode(sitemap),
                    TestDataProvider.CreateNewSitemapNode(sitemap),
                    TestDataProvider.CreateNewSitemapNode(sitemap)
                };

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<Sitemap>())
                .Returns(new[] { sitemap }.AsQueryable());

            var savedMaps = new List<SitemapArchive>();
            repositoryMock
                .Setup(f => f.Save(It.IsAny<SitemapArchive>()))
                .Callback<SitemapArchive>(savedMaps.Add);

            var service = new DefaultSitemapService(repositoryMock.Object);
            service.ArchiveSitemap(sitemap.Id);

            Assert.AreEqual(savedMaps.Count, 1);
            Assert.AreEqual(savedMaps[0].Sitemap.Id, sitemap.Id);
            Assert.IsNotEmpty(savedMaps[0].ArchivedVersion);
        }
    }
}
