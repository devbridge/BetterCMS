using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Command.Widget.DeleteWidget;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Services;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.WidgetTests
{
    [TestFixture]
    public class DeleteWidgetCommandTest : IntegrationTestBase
    {
        [Test]
        public void Should_Delete_ServerControlWidget_Successfully()
        {
            var serverControlWidget = TestDataProvider.CreateNewServerControlWidget();

            RunActionInTransaction(session =>
                {
                    session.SaveOrUpdate(serverControlWidget);
                    session.Flush();
                    session.Clear();

                    var uow = new DefaultUnitOfWork(session);
                    var repository = new DefaultRepository(uow);
                    var optionService = new Mock<IOptionService>().Object;
                    var contentService = new Mock<IContentService>().Object;
                    var childContentService = new Mock<IChildContentService>().Object;
                    var categoryService = new Mock<ICategoryService>();
                    var cmsConfiguration = new Mock<ICmsConfiguration>().Object;
                    var widgetService = new DefaultWidgetService(repository, uow, optionService, contentService, childContentService, categoryService.Object, cmsConfiguration);

                    DeleteWidgetCommand command = new DeleteWidgetCommand(widgetService);

                    bool success = command.Execute(new DeleteWidgetRequest
                                                       {
                                                           WidgetId = serverControlWidget.Id,
                                                           Version = serverControlWidget.Version
                                                       });
                    Assert.IsTrue(success);
                });
        }

        [Test]
        public void Should_Delete_HtmlContentWidget_Successfully()
        {
            var htmlContentWidget = TestDataProvider.CreateNewHtmlContentWidget();

            RunActionInTransaction(session =>
            {
                session.SaveOrUpdate(htmlContentWidget);
                session.Flush();
                session.Clear();

                var uow = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(uow);
                var optionService = new Mock<IOptionService>().Object;
                var contentService = new Mock<IContentService>().Object;
                var childContentService = new Mock<IChildContentService>().Object;
                var categoryService = new Mock<ICategoryService>();
                var cmsConfiguration = new Mock<ICmsConfiguration>().Object;
                var widgetService = new DefaultWidgetService(repository, uow, optionService, contentService, childContentService, categoryService.Object, cmsConfiguration);

                DeleteWidgetCommand command = new DeleteWidgetCommand(widgetService);

                bool success = command.Execute(new DeleteWidgetRequest
                                                    {
                                                        WidgetId = htmlContentWidget.Id,
                                                        Version = htmlContentWidget.Version
                                                    });
                Assert.IsTrue(success);
            });
        }
    }
}
