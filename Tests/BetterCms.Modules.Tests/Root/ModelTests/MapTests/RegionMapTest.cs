// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionMapTest.cs" company="Devbridge Group LLC">
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
using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class RegionMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Region_Successfully()
        {
            var entity = TestDataProvider.CreateNewRegion();
            RunEntityMapTestsInTransaction(entity);
        }

        [Test]
        public void Should_Insert_And_Retrieve_Region_PageContentOptions_Successfully()
        {
            var region = TestDataProvider.CreateNewRegion();

            var pageContents = new[]
                {
                    TestDataProvider.CreateNewPageContent(null, null, region)
                };

            region.PageContents = pageContents;

            SaveEntityAndRunAssertionsInTransaction(
                region,
                result =>
                {
                    Assert.AreEqual(region, result);
                    Assert.AreEqual(pageContents.OrderBy(f => f.Id), result.PageContents.OrderBy(f => f.Id));
                });
        }
        
        [Test]
        public void Should_Insert_And_Retrieve_Region_LayoutRegions_Successfully()
        {
            var region = TestDataProvider.CreateNewRegion();

            var layoutRegions = new[]
                {
                    TestDataProvider.CreateNewLayoutRegion(null, region),
                    TestDataProvider.CreateNewLayoutRegion(null, region)
                };

            region.LayoutRegion = layoutRegions;

            SaveEntityAndRunAssertionsInTransaction(
                region,
                result =>
                {
                    Assert.AreEqual(region, result);
                    Assert.AreEqual(layoutRegions.OrderBy(f => f.Id), result.LayoutRegion.OrderBy(f => f.Id));
                });
        }
    }
}
