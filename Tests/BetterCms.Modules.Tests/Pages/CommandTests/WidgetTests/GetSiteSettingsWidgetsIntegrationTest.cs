using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Command.Widget.GetSiteSettingsWidgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using MvcContrib.Sorting;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.WidgetTests
{
    [TestFixture]
    public class GetSiteSettingsWidgetsIntegrationTest : IntegrationTestBase
    {
        [Test]
        public void Should_Retrieve_Widgets_From_Database_Paged_And_Sorted_By_CategoryName()
        {            
            RunActionInTransaction(session =>
                {
                    var widgets = new Widget[]
                                      {
                                         this.TestDataProvider.CreateNewServerControlWidget(),
                                         this.TestDataProvider.CreateNewServerControlWidget(),
                                         this.TestDataProvider.CreateNewHtmlContentWidget()
                                      };
                    int i = 0;
                    foreach (var widget in widgets)
                    {
                        widget.Name = "test name " + i++;
                        session.SaveOrUpdate(widget);
                    }
                    session.Flush();
                    session.Clear();

                    IUnitOfWork unitOfWork = new DefaultUnitOfWork(session);

                    GetSiteSettingsWidgetsCommand command = new GetSiteSettingsWidgetsCommand();                    
                    command.UnitOfWork = unitOfWork;
                    command.Repository = new DefaultRepository(unitOfWork);

                    var result = command.Execute(new SearchableGridOptions
                                                     {
                                                         PageSize = 20,
                                                         Column = "CategoryName",
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
