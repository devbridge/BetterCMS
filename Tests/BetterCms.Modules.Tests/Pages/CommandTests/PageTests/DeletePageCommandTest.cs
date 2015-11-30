// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeletePageCommandTest.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Security;

using BetterCms.Module.Pages.Command.Page.DeletePage;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.PageTests
{
    [TestFixture]
    public class DeletePageCommandTest : IntegrationTestBase
    {
        [Test]
        public void Sould_Delete_Page_Successfully()
        {
            RunActionInTransaction(session =>
                {
                    const string url = "/test-link/";
                    var uow = new DefaultUnitOfWork(session);
                    var repository = new DefaultRepository(uow);
                    
                    var page = TestDataProvider.CreateNewPageWithTagsContentsOptionsAndAccessRules(session);
                    
                    session.SaveOrUpdate(page);
                    session.Flush();
                    session.Clear();
                    
                    var sitemapService = new Mock<ISitemapService>();
                    sitemapService
                        .Setup(service => service.GetNodesByPage(It.IsAny<PageProperties>()))
                        .Returns(() => new List<SitemapNode>());

                    var urlService = new Mock<IUrlService>();
                    urlService.Setup(f => f.FixUrl(It.IsAny<string>())).Returns((string a) => a);

                    var securityService = new Mock<ICmsSecurityConfiguration>();
                    securityService.Setup(f => f.AccessControlEnabled).Returns(false);

                    var configurationService = new Mock<ICmsConfiguration>();
                    configurationService.Setup(f => f.Security).Returns(securityService.Object);

                    var redirectService = new Mock<IRedirectService>();

                    var accessControlService = new Mock<IAccessControlService>();

                    var pageService = new DefaultPageService(repository,
                        redirectService.Object,
                        urlService.Object,
                        accessControlService.Object,
                        configurationService.Object,
                        sitemapService.Object,
                        uow);

                    var command = new DeletePageCommand(pageService);
                    command.Repository = repository;
                    command.UnitOfWork = uow;
                    command.Context = new Mock<ICommandContext>().Object;

                    var result = command.Execute(new DeletePageViewModel
                                        {
                                            PageId = page.Id,
                                            UpdateSitemap = false,
                                            RedirectUrl = null,
                                            SecurityWord = "DELETE",
                                            Version = page.Version
                                        });

                    Assert.IsTrue(result);
                    session.Clear();

                    var actual = repository.AsQueryable<PageProperties>().FirstOrDefault(f => f.Id == page.Id && !f.IsDeleted);                    
                    Assert.IsNull(actual);   
                });
        }
    }
}
