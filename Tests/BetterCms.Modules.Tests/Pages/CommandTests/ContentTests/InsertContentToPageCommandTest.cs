using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Module.Pages.Command.Content.InsertContent;
using BetterCms.Module.Root.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.ContentTests
{
    [TestFixture]
    public class InsertContentToPageCommandTest : TestBase
    {
        [Test]
        public void Should_Insert_HtmlWidget_To_Page_Successfully()
        {
            // Create html content
            var widget = TestDataProvider.CreateNewHtmlContentWidget();
            widget.Status = ContentStatus.Published;

            var page = TestDataProvider.CreateNewPage();
            var region = TestDataProvider.CreateNewRegion();
            var pageContent = TestDataProvider.CreateNewPageContent(widget, page, region);
            pageContent.Order = TestDataProvider.ProvideRandomNumber(1, 99999);

            // Mock
            var repository = new Mock<IRepository>();
            repository
                .Setup(r => r.AsProxy<Page>(It.IsAny<Guid>()))
                .Returns(TestDataProvider.CreateNewPage());
            repository
                .Setup(r => r.AsProxy<Region>(It.IsAny<Guid>()))
                .Returns(TestDataProvider.CreateNewRegion());
            repository
                .Setup(r => r.AsQueryable<PageContent>())
                .Returns(new List<PageContent> { pageContent }.AsQueryable());
            repository
                .Setup(r => r.Save(It.IsAny<PageContent>()))
                .Callback<PageContent>(pc =>
                              {
                                  Assert.AreEqual(pc.Order, pageContent.Order + 1);
                              });

            // Create command
            var command = new InsertContentToPageCommand();
            command.UnitOfWork = new Mock<IUnitOfWork>().Object;
            command.Repository = repository.Object;

            // Execute
            var request = new InsertContentToPageRequest { ContentId = widget.Id, PageId = page.Id, RegionId = region.Id };
            var result = command.Execute(request);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }
    }
}
