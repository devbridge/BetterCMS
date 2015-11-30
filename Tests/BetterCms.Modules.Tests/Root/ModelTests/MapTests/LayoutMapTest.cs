// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LayoutMapTest.cs" company="Devbridge Group LLC">
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
    public class LayoutMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Layout_Successfully()
        {
            var layout = TestDataProvider.CreateNewLayout();
            RunEntityMapTestsInTransaction(layout);
        }

        [Test]
        public void Should_Insert_And_Retrieve_Layout_Pages_Successfully()
        {
            var layout = TestDataProvider.CreateNewLayout();
            var pages = new[]
                {
                    TestDataProvider.CreateNewPage(layout),
                    TestDataProvider.CreateNewPage(layout),
                    TestDataProvider.CreateNewPage(layout)                    
                };
            layout.Pages = pages;

            SaveEntityAndRunAssertionsInTransaction(
                layout,
                result =>
                    {
                        Assert.AreEqual(layout, result);
                        Assert.AreEqual(pages.OrderBy(f => f.Id), result.Pages.OrderBy(f => f.Id));
                    });
        }

        [Test]
        public void Should_Insert_And_Retrieve_Layout_LayoutRegions_Successfully()
        {
            var layout = TestDataProvider.CreateNewLayout();
            var layoutRegions = new[]
                {
                    TestDataProvider.CreateNewLayoutRegion(layout),
                    TestDataProvider.CreateNewLayoutRegion(layout),
                    TestDataProvider.CreateNewLayoutRegion(layout)                    
                };
            layout.LayoutRegions = layoutRegions;

            SaveEntityAndRunAssertionsInTransaction(
                layout,
                result =>
                {
                    Assert.AreEqual(layout, result);
                    Assert.AreEqual(layoutRegions.OrderBy(f => f.Id), result.LayoutRegions.OrderBy(f => f.Id));
                });
        }
    }
}
