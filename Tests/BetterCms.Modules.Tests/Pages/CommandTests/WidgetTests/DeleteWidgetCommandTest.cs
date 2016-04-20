// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteWidgetCommandTest.cs" company="Devbridge Group LLC">
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

                    var command = new DeleteWidgetCommand(widgetService);

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

                var command = new DeleteWidgetCommand(widgetService);

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
