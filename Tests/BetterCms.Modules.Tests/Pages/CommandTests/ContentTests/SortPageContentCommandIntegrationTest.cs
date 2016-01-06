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
                SortPageContentCommand command = new SortPageContentCommand(configuration);
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
