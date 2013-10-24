using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Command.Widget.DeleteWidget;

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

                    DeleteWidgetCommand command = new DeleteWidgetCommand();
                    command.UnitOfWork = new DefaultUnitOfWork(session);
                    command.Repository = new DefaultRepository(command.UnitOfWork);

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
                
                DeleteWidgetCommand command = new DeleteWidgetCommand();
                command.UnitOfWork = new DefaultUnitOfWork(session);
                command.Repository = new DefaultRepository(command.UnitOfWork);

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
