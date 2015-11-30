// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetSiteSettingsWidgetsIntegrationTest.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Pages.Command.Widget.GetSiteSettingsWidgets;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using Moq;

using MvcContrib.Sorting;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.WidgetTests
{
    [TestFixture]
    public class GetSiteSettingsWidgetsIntegrationTest : IntegrationTestBase
    {
        [Test]
        public void Should_Retrieve_Widgets_From_Database_Paged_And_Sorted_By_WidgetName()
        {            
            RunActionInTransaction(session =>
                {
                    var widgets = new Widget[]
                                      {
                                         TestDataProvider.CreateNewServerControlWidget(),
                                         TestDataProvider.CreateNewServerControlWidget(),
                                         TestDataProvider.CreateNewHtmlContentWidget()
                                      };
                    int i = 0;
                    foreach (var widget in widgets)
                    {
                        widget.Name = "test name " + i++;
                        session.SaveOrUpdate(widget);
                    }
                    session.Flush();
                    session.Clear();

                    var unitOfWork = new DefaultUnitOfWork(session);
                    var repository = new DefaultRepository(unitOfWork);
                    var categoryService = new Mock<ICategoryService>();
                    var cmsConfiguration = new Mock<ICmsConfiguration>().Object;
                    var widgetService = new DefaultWidgetService(repository, unitOfWork, null, null, null, categoryService.Object, cmsConfiguration);
                    var command = new GetSiteSettingsWidgetsCommand(widgetService);

                    var result = command.Execute(new WidgetsFilter
                                                     {
                                                         PageSize = 20,
                                                         Column = "WidgetName",
                                                         Direction = SortDirection.Ascending,
                                                         PageNumber = 1,
                                                         SearchQuery = "test name 2"
                                                     });

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, result.Items.TotalItems);
                    Assert.AreEqual(widgets[2].Name, result.Items.First().WidgetName);
                });
        }
    }
}
