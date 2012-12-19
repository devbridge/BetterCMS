using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Command.Content.SortPageContent;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;

namespace BetterCms.Test.Module.Pages.CommandTests.ContentTests
{

    [TestFixture]
    public class SortPageContentCommandIntegrationTest : DatabaseTestBase
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

                SortPageContentCommand command = new SortPageContentCommand();
                command.UnitOfWork = unitOfWork;
                command.Repository = new DefaultRepository(unitOfWork);

                var request = new PageContentSortViewModel
                    {
                        PageId = page.Id,
                        RegionId = region.Id,
                        PageContents =
                            new List<ContentViewModel>
                                {
                                    new ContentViewModel { Id = page.PageContents[2].Id, Version = page.PageContents[2].Version },
                                    new ContentViewModel { Id = page.PageContents[1].Id, Version = page.PageContents[1].Version },
                                    new ContentViewModel { Id = page.PageContents[0].Id, Version = page.PageContents[0].Version },
                                }
                    };
                var response = command.Execute(request);

                Assert.AreEqual(2, response.UpdatedPageContents.Count);

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
