// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageMapTest.cs" company="Devbridge Group LLC">
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
    public class PageMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Page_Successfully()
        {            
            var entity = TestDataProvider.CreateNewPage();
            RunEntityMapTestsInTransaction(entity);
        }

        [Test]
        public void Should_Insert_And_Retrieve_Page_PageContents_Successfully()
        {
            var page = TestDataProvider.CreateNewPage();

            var pageContents = new[]
                {
                    TestDataProvider.CreateNewPageContent(null, page),
                    TestDataProvider.CreateNewPageContent(null, page),
                    TestDataProvider.CreateNewPageContent(null, page)
                };

            page.PageContents = pageContents;
            
            var pageOptions = new[]
                {
                    TestDataProvider.CreateNewPageOption(page),
                    TestDataProvider.CreateNewPageOption(page),
                    TestDataProvider.CreateNewPageOption(page)
                };

            page.Options = pageOptions;

            SaveEntityAndRunAssertionsInTransaction(
                page,
                result =>
                {
                    Assert.AreEqual(page, result);
                    Assert.AreEqual(pageContents.OrderBy(f => f.Id), result.PageContents.OrderBy(f => f.Id));
                });
        }       
    }
}
