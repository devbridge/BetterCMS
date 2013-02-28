using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.DataServices;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServicesTests
{
    [TestFixture]
    public class DefaultWidgetApiServiceTest : TestBase
    {
        private Guid pageId = new Guid();

        [Test]
        public void Should_Return_History_List_Successfully()
        {
            var widgets = CreateFakeWidgets();
            var repositoryMock = MockRepository(widgets);

            var service = new DefaultWidgetApiService(repositoryMock.Object);

            var htmlWidget = service.GetHtmlContentWidget(new Guid());
            Assert.IsNotNull(htmlWidget);

            var serverWidget = service.GetServerControlWidget(new Guid());
            Assert.IsNotNull(serverWidget);

            var allWidgets = service.GetWidgets();
            Assert.IsNotNull(allWidgets);
            Assert.AreEqual(widgets.Count(), allWidgets.Count());

            var pageWidgets = service.GetPageWidgets(pageId);
            Assert.IsNotNull(pageWidgets);
            Assert.AreEqual(1, pageWidgets.Count());
        }

        private Widget[] CreateFakeWidgets()
        {
            var serverControlWidget = TestDataProvider.CreateNewServerControlWidget();
            serverControlWidget.PageContents = new List<PageContent>() { new PageContent() { Page = new Page() { Id = pageId } } };

            var htmlContentWidget = TestDataProvider.CreateNewHtmlContentWidget();

            return new Widget[]
                {
                    serverControlWidget,
                    htmlContentWidget
                };
        }

        private static Mock<IRepository> MockRepository(Widget[] widgets)
        {
            var mock = new Mock<IRepository>();

            mock.Setup(r => r.First<ServerControlWidget>(It.IsAny<Guid>()))
                .Returns((ServerControlWidget)widgets[0]);

            mock.Setup(r => r.First<HtmlContentWidget>(It.IsAny<Guid>()))
                .Returns((HtmlContentWidget)widgets[1]);

            mock.Setup(f => f.AsQueryable<Widget>())
                .Returns(widgets.AsQueryable());

            return mock;
        }
    }
}
