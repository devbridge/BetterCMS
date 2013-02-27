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
                var service = new DefaultContentApiService(repository);

                var pageId = new Guid("66d2dc73-9908-4a54-ad6b-a15f00c50f91");

//                var contents1 = service.GetPageContents(pageId, p => ((PageProperties)p.Page).CustomCss.Transliterate(true) == "agfa");
//                var contents2 = service.GetRegionContents(pageId, contents1[0].Region.Id);
//                var contents3 = service.GetRegionContents(pageId, contents1[0].Region.RegionIdentifier);
//                var content = service.GetContent(contents1[0].Content.Id);
//                var pageContent = service.GetPageContent(contents1[0].Id);

//                var result = service.GetFile(folder.Id);



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
