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
