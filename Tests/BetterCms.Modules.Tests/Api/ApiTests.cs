using System;
using System.Linq;
using System.Text;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.DataServices;
using BetterCms.Module.MediaManager.DataServices;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Navigation.DataServices;
using BetterCms.Module.Navigation.Services;
using BetterCms.Module.Pages.DataServices;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc.Helpers;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api
{
    [TestFixture]
    public class ApiTests : DatabaseTestBase
    {
        [Test]
        public void Api_Tests()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);
                var service = new DefaultHistoryApiService(new DefaultHistoryService(repository));

//                var contentId = new Guid("5CFA685B-DE66-414A-A9F4-A17200AE5D90");
//
//                var contents1 = service.GetContentHistory(contentId);
//                var contents2 = service.GetContentHistory(contentId, c => c is HtmlContent && ((HtmlContent)c).Html.ToLower().Contains("draft"));
                //var contents3 = service.GetContentHistory(contentId, null, c => c.Name, true);

                //contentId = new Guid("DBC09596-572A-44D5-AC9C-A17200EBA112");
                //var results = service.GetContentHistory(contentId);


//                var test = service;

//                var widgetApi = new DefaultWidgetApiService(repository);
//                var widgets = widgetApi.GetPageWidgets(pageId);
//                var result = new StringBuilder("");
//                foreach (var widget in widgets)
//                {
//                    result.AppendLine(string.Format("{0}: {1}, page options count: {2}", widget.Name, widget.Category != null ? widget.Category.Name : "[unknown category]", widget.PageContents.Where(p => p.Page.Id == pageId).Sum(p => p.Options.Count)));
//                }
//                var x = result.ToString();
                
//                var test = contents1;
            });
        }
    }
}
