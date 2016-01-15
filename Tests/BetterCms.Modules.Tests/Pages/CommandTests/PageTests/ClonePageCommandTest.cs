// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClonePageCommandTest.cs" company="Devbridge Group LLC">
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

using Autofac;

using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Command.Page.ClonePage;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.PageTests
{
    [TestFixture]
    public class ClonePageCommandTest : IntegrationTestBase
    {
        [Test]
        public void Should_Clone_Page_With_Tags_Options_Contents_AccessRules()
        {
            RunActionInTransaction(session =>
                {
                    const string url = "/test-link";
                    var uow = new DefaultUnitOfWork(session);
                    var repository = new DefaultRepository(uow);

                    var pageToClone = TestDataProvider.CreateNewPageWithTagsContentsOptionsAndAccessRules(session, 2, 2, 2, 2);
                    
                    session.SaveOrUpdate(pageToClone);
                    session.Flush();
                    session.Clear();
                    
                    var pageService = new Mock<IPageService>();
                    pageService.Setup(f => f.ValidatePageUrl(It.IsAny<string>(), It.IsAny<Guid?>()));
                    pageService.Setup(f => f.CreatePagePermalink(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>())).Returns(url);

                    var urlService = new Mock<IUrlService>();
                    urlService.Setup(f => f.FixUrl(It.IsAny<string>())).Returns(url);

                    var rules = new List<IAccessRule>();
                    var rule1 = TestDataProvider.CreateNewAccessRule();
                    rules.Add(rule1);
                    var rule2 = TestDataProvider.CreateNewAccessRule();
                    rules.Add(rule2);

                    var pageCloningService = new DefaultPageCloneService(pageService.Object, urlService.Object, 
                        new Mock<ISecurityService>().Object, new Mock<IAccessControlService>().Object, repository, uow, 
                        Container.Resolve<ICmsConfiguration>());

                    var command = new ClonePageCommand(pageCloningService, Container.Resolve<ICmsConfiguration>());
                    command.Repository = repository;
                    command.UnitOfWork = uow;

                    var result = command.Execute(new ClonePageViewModel
                                        {
                                            PageId = pageToClone.Id,
                                            PageTitle = "new cloned page",
                                            PageUrl = url,
                                            UserAccessList = pageToClone.AccessRules.Select(u => new UserAccessViewModel(u)).ToList()
                                        });

                    Assert.IsNotNull(result);
                    session.Clear();

                    var actual = repository.AsQueryable<PageProperties>().Where(f => f.Id == result.PageId).ToList().FirstOrDefault();
                    
                    Assert.IsNotNull(actual);
                    Assert.AreEqual(2, actual.AccessRules.Count(), "AccessRules");
                    Assert.AreEqual(2, actual.PageTags.Count(), "Tags");
                    Assert.AreEqual(2, actual.PageContents.Count(), "Contents");
                    Assert.AreEqual(2, actual.Options.Count(), "Options");
                });
        }
    }
}
