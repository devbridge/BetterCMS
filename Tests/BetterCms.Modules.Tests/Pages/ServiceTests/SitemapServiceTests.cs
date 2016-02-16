// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapServiceTests.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;
using BetterModules.Core.DataAccess;
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

            var repositoryMock = new Mock<IRepository>();
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

            var cmsConfigurationMock = new Mock<ICmsConfiguration>();
            cmsConfigurationMock.Setup(f => f.EnableMacros).Returns(false);

            var service = new DefaultSitemapService(repositoryMock.Object, null, cmsConfigurationMock.Object, null);
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
