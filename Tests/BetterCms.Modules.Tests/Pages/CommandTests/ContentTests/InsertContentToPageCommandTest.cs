using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Pages.Accessors;
using BetterCms.Module.Pages.Command.Content.InsertContent;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Dependencies;

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
            pageContent.Id = Guid.NewGuid();
            pageContent.Version = 10;
            widget.Version = 20;

            // Mock content Service
            var contentService = new Mock<IContentService>();
            contentService
                .Setup(r => r.GetPageContentNextOrderNumber(It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(pageContent.Order + 1);

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
                .Setup(r => r.AsQueryable<Content>())
                .Returns(new List<Content> { widget }.AsQueryable());
            repository
                .Setup(r => r.Save(It.IsAny<PageContent>()))
                .Callback<PageContent>(pc =>
                              {
                                  Assert.AreEqual(pc.Order, pageContent.Order + 1);
                                  pc.Id = pageContent.Id;
                                  pc.Version = pageContent.Version;
                              });

            // Create command
            var command = new InsertContentToPageCommand(contentService.Object,
                new FakePageContentProjectionFactory(null, null),
                new Mock<IWidgetService>().Object);
            command.UnitOfWork = new Mock<IUnitOfWork>().Object;
            command.Repository = repository.Object;

            // Execute
            var request = new InsertContentToPageRequest { ContentId = widget.Id, PageId = page.Id, RegionId = region.Id };
            var result = command.Execute(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.PageContentId, pageContent.Id);
            Assert.AreEqual(result.PageId, page.Id);
            Assert.AreEqual(result.ContentId, widget.Id);
            Assert.AreEqual(result.ContentType, "html-widget");
            Assert.AreEqual(result.ContentVersion, widget.Version);
            Assert.AreEqual(result.PageContentVersion, pageContent.Version);
            Assert.AreEqual(result.Title, widget.Name);
            Assert.AreEqual(result.RegionId, region.Id);
            Assert.AreEqual(result.DesirableStatus, widget.Status);
        }

        private class FakePageContentProjectionFactory : PageContentProjectionFactory
        {
            public FakePageContentProjectionFactory(PerWebRequestContainerProvider containerProvider, IUnitOfWork unitOfWork)
                : base(containerProvider, unitOfWork)
            {
            }

            public override IContentAccessor GetAccessorForType(IContent content, IList<IOptionValue> options = null)
            {
                return new HtmlContentWidgetAccessor((HtmlContentWidget)content, options);
            }
        }
    }
}
