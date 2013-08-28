using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autofac;

using BetterCms.Core;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Pages.Command.Page.CreatePage;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using Moq;

using NHibernate.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.PageTests
{
    [TestFixture]
    public class CreatePageCommandTest : DatabaseTestBase
    {
        [Test]
        public void Should_Create_New_Page_With_Access_Rules()
        {
            RunActionInTransaction(
                session =>
                    {
                        var tempLayout = TestDataProvider.CreateNewLayout();
                        session.SaveOrUpdate(tempLayout);
                        session.Flush();

                        var uow = new DefaultUnitOfWork(session);
                        var repository = new DefaultRepository(uow);
                        var configMock = new Mock<ICmsConfiguration>();
                        configMock.SetupAllProperties().Setup(f => f.AccessControlEnabled).Returns(true);
                        var config = configMock.Object;

                        var command = new CreatePageCommand(
                            new Mock<IPageService>().SetupAllProperties().Object,
                            new DefaultUrlService(uow, config),
                            config,
                            new DefaultAccessControlService(repository, new HttpRuntimeCacheService(), config),
                            new Mock<IOptionService>().SetupAllProperties().Object);

                        command.UnitOfWork = uow;
                        command.Repository = repository;

                        var request = new AddNewPageViewModel();
                        request.AccessControlEnabled = true;
                        request.PageTitle = "TestCreatePageCommand";
                        request.PageUrl = "/test-CreatePageCommand/";
                        request.TemplateId = tempLayout.Id;

                        request.UserAccessList = new List<UserAccessViewModel>(
                            new[]
                                {
                                    new UserAccessViewModel
                                        {
                                            Identity = "test 1",
                                            AccessLevel = AccessLevel.ReadWrite
                                        },

                                    new UserAccessViewModel
                                        {
                                            Identity = "test 2",
                                            AccessLevel = AccessLevel.NoPermissions
                                        }
                                });

                        var response = command.Execute(request);
                        session.Clear();

                        Assert.IsNotNull(response);
                        
                        var page = session.Query<Page>().FirstOrDefault(f => f.Id == response.PageId);
                        Assert.IsNotNull(page);
                        Assert.IsNotNull(page.AccessRules);
                        Assert.AreEqual(2, page.AccessRules.Count());

                    });
        }
    }
}
