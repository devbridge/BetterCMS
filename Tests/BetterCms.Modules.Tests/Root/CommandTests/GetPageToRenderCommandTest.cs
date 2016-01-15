// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPageToRenderCommandTest.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.CommandTests
{
    [TestFixture]
    public class GetPageToRenderCommandTest : IntegrationTestBase
    {
        [Test]
        public void Should_Return_Page_With_PageContents_Successfully()
        {
            string virtualPath = "/test/" + Guid.NewGuid().ToString().Replace("-", string.Empty) + "/";
            var layout = TestDataProvider.CreateNewLayout();
            var regionA = TestDataProvider.CreateNewRegion();
            var regionB = TestDataProvider.CreateNewRegion();

            var htmlContent = TestDataProvider.CreateNewHtmlContent();
            var serverWidgetContent = TestDataProvider.CreateNewServerControlWidget();
            var htmlContentWidget = TestDataProvider.CreateNewHtmlContentWidget();

            layout.LayoutRegions = new List<LayoutRegion>
                                       {
                                           TestDataProvider.CreateNewLayoutRegion(layout, regionA),
                                           TestDataProvider.CreateNewLayoutRegion(layout, regionB)
                                       };

            var page = TestDataProvider.CreateNewPageProperties(layout);
            page.PageContents = new List<PageContent>
                                    {
                                        TestDataProvider.CreateNewPageContent(htmlContent, page, regionA), 
                                        TestDataProvider.CreateNewPageContent(serverWidgetContent, page, regionB),
                                        TestDataProvider.CreateNewPageContent(htmlContentWidget, page, regionB)
                                    };
            page.PageUrl = virtualPath;

            RunDatabaseActionAndAssertionsInTransaction(
                page, 
                session =>
                    { 
                        session.Save(page);
                        session.Flush();
                    },
                (result, session) =>
                    {
                        Page pageAlias = null;
                        Layout layoutAlias = null;

                        var entity = session.QueryOver(() => pageAlias)
                          .Inner.JoinAlias(() => pageAlias.Layout, () => layoutAlias)
                          .Where(f => f.PageUrl == virtualPath.ToLowerInvariant())
                          .Fetch(f => f.Layout).Eager
                          .Fetch(f => f.Layout.LayoutRegions).Eager
                          .Fetch(f => f.PageContents).Eager
                          .Fetch(f => f.PageContents[0].Content).Eager
                          .SingleOrDefault();

                        Assert.IsNotNull(entity);
                        Assert.AreEqual(page.PageContents.Count(), entity.PageContents.Distinct().Count());
                        Assert.AreEqual(page.Layout.LayoutRegions.Count(), entity.Layout.LayoutRegions.Distinct().Count());
                    });
        }
    }
}
