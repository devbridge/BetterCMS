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
        public void Should_Archive_Unarchive_Sitemap_Successfully()
        {
            var sitemap = TestDataProvider.CreateNewSitemap();
            sitemap.Nodes = new List<SitemapNode>
                {
                    TestDataProvider.CreateNewSitemapNode(sitemap),
                    TestDataProvider.CreateNewSitemapNode(sitemap),
                    TestDataProvider.CreateNewSitemapNode(sitemap)
                };
            var rootNode = TestDataProvider.CreateNewSitemapNode(sitemap);
            foreach (var node in sitemap.Nodes)
            {
                node.ParentNode = rootNode;
            }
            sitemap.Nodes.Add(rootNode);

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<Sitemap>())
                .Returns(new[] { sitemap }.AsQueryable());

            var savedMaps = new List<SitemapArchive>();
            repositoryMock
                .Setup(f => f.Save(It.IsAny<SitemapArchive>()))
                .Callback<SitemapArchive>(savedMaps.Add);

            repositoryMock
                .Setup(f => f.AsQueryable<SitemapArchive>())
                .Returns(savedMaps.AsQueryable);

            Mock<ICmsConfiguration> cmsConfigurationMock = new Mock<ICmsConfiguration>();
            cmsConfigurationMock.Setup(f => f.EnableMacros).Returns(false);

            var service = new DefaultSitemapService(repositoryMock.Object, cmsConfigurationMock.Object);
            service.ArchiveSitemap(sitemap.Id);

            Assert.AreEqual(savedMaps.Count, 1);
            Assert.AreEqual(savedMaps[0].Sitemap.Id, sitemap.Id);
            Assert.IsNotEmpty(savedMaps[0].ArchivedVersion);

            var unarchivedSitemap = service.GetArchivedSitemapVersionForPreview(savedMaps[0].Id);
            Assert.IsNotNull(unarchivedSitemap);
            Assert.AreEqual(sitemap.Nodes.Count, unarchivedSitemap.Nodes.Count);

            unarchivedSitemap.Nodes = unarchivedSitemap.Nodes.OrderBy(node => node.Title).ToList();
            sitemap.Nodes = sitemap.Nodes.OrderBy(node => node.Title).ToList();

            for (var i = 0; i < sitemap.Nodes.Count; i++)
            {
                var orig = sitemap.Nodes[i];
                var unarchived = unarchivedSitemap.Nodes[i];

                Assert.AreEqual(orig.Title, unarchived.Title);
                Assert.AreEqual(orig.Url, unarchived.Url);
                Assert.AreEqual(orig.UrlHash, unarchived.UrlHash);
            }
        }
    }
}
