// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageContentProjectionTest.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Accessors;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Services.Caching;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ProjectionTests
{
    [TestFixture]
    public class PageContentProjectionTest : SerializationTestBase
    {
        [Test]
        public void Should_By_Xml_And_Binary_Serializable()
        {
            var pageContent = TestDataProvider.CreateNewPageContent();
            pageContent.Content = TestDataProvider.CreateNewHtmlContent();
            pageContent.Options = new[]
                                                 {
                                                     TestDataProvider.CreateNewPageContentOption(pageContent),
                                                     TestDataProvider.CreateNewPageContentOption(pageContent),
                                                     TestDataProvider.CreateNewPageContentOption(pageContent)
                                                 };

            var cmsConfiguration = new Mock<ICmsConfiguration>();
            var optionService = new DefaultOptionService(null, new HttpRuntimeCacheService(), cmsConfiguration.Object);
            var optionValues = optionService.GetMergedOptionValues(pageContent.Options, null);

            var original = new PageContentProjection(
                pageContent, pageContent.Content, new HtmlContentAccessor((HtmlContent)pageContent.Content, optionValues));

            RunSerializationAndDeserialization(original,
                projection =>
                    {
                        Assert.AreEqual(original.ContentId, projection.ContentId);
                        Assert.AreEqual(original.Order, projection.Order);
                        Assert.AreEqual(original.RegionId, projection.RegionId);
                    });
        }
    }
}
