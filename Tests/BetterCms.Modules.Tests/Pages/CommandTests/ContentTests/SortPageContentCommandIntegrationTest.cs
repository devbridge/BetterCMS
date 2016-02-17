// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SortPageContentCommandIntegrationTest.cs" company="Devbridge Group LLC">
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
using System.Security.Principal;

using Autofac;

using BetterCms.Core.Security;

using BetterCms.Module.Pages.Command.Content.SortPageContent;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.ContentTests
{

    [TestFixture]
    public class SortPageContentCommandIntegrationTest : IntegrationTestBase
    {
        [Test]
        public void Should_Sort_Page_Content_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var layout = TestDataProvider.CreateNewLayout();
                var region = TestDataProvider.CreateNewRegion();

                layout.LayoutRegions = new List<LayoutRegion>
                    {
                        TestDataProvider.CreateNewLayoutRegion(layout, region),
                    };

                var page = TestDataProvider.CreateNewPageProperties(layout);
                page.PageContents = new[]
                    {
                        TestDataProvider.CreateNewPageContent(TestDataProvider.CreateNewHtmlContent(), page, region),
                        TestDataProvider.CreateNewPageContent(TestDataProvider.CreateNewHtmlContent(), page, region),
                        TestDataProvider.CreateNewPageContent(TestDataProvider.CreateNewHtmlContent(), page, region)
                    };

                session.SaveOrUpdate(page);
                session.Flush();
                session.Clear();

                IUnitOfWork unitOfWork = new DefaultUnitOfWork(session);

                var configuration = Container.Resolve<ICmsConfiguration>();
                var command = new SortPageContentCommand(configuration);
                command.UnitOfWork = unitOfWork;
                command.Repository = new DefaultRepository(unitOfWork);
                command.Context = new Mock<ICommandContext>().Object;

                var accessControlService = new Mock<IAccessControlService>();
                accessControlService.Setup(s => s.DemandAccess(It.IsAny<IPrincipal>(), It.IsAny<string>()));
                command.AccessControlService = accessControlService.Object;

                var request = new PageContentSortViewModel
                    {
                        PageId = page.Id,
                        PageContents =
                            new List<ContentSortViewModel>
                                {
                                    new ContentSortViewModel { RegionId = region.Id, PageContentId = page.PageContents[2].Id, Version = page.PageContents[2].Version },
                                    new ContentSortViewModel { RegionId = region.Id, PageContentId = page.PageContents[1].Id, Version = page.PageContents[1].Version },
                                    new ContentSortViewModel { RegionId = region.Id, PageContentId = page.PageContents[0].Id, Version = page.PageContents[0].Version },
                                }
                    };
                var response = command.Execute(request);

                Assert.IsNotNull(response);
                Assert.IsNotNull(response.PageContents);
                Assert.AreEqual(response.PageContents.Count, request.PageContents.Count);

                session.Flush();
                session.Clear();

                var updatedPage = command.Repository.AsQueryable<PageContent>(f => f.Page.Id == page.Id).ToList();

                Assert.AreEqual(2, updatedPage.FirstOrDefault(f => f.Id == page.PageContents[0].Id).Order);
                Assert.AreEqual(1, updatedPage.FirstOrDefault(f => f.Id == page.PageContents[1].Id).Order);
                Assert.AreEqual(0, updatedPage.FirstOrDefault(f => f.Id == page.PageContents[2].Id).Order);
            });
        }
    }
}
