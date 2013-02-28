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
using BetterCms.Module.Pages.DataContracts.Enums;
using BetterCms.Module.Pages.DataServices;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Linq;

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
                var service = new DefaultLayoutApiService(repository, unitOfWork);

                //var pages1 = service.GetPages();
                //var pages2 = service.GetPages(loadChilds:PageLoadableChilds.All);
                var layouts = service.GetLayouts();
                var layouts4 = service.GetLayouts(itemsPerPage:2, pageNumber:2);
                var layouts1 = service.GetLayouts(l => l.Name.Contains("Column"), l => l.Name, true);
                var layouts2 = service.GetLayouts(l => (l.Name == "Default Wide" || l.Name == "Default Two Columns"), l => l.Name, true, itemsPerPage:1);
                //var regions = service.GetRegions();
                //var layoutRegions = service.GetLayoutRegions(layouts[0].Id);

                /*var a = layouts[0].LayoutRegions.Count;
                var b = layouts[0].LayoutRegions[0];
                var c = layouts[1].LayoutRegions.Count;*/

                //var pageId = new Guid("B0326B23-D0C0-4B4A-B7EE-A17200AE46BE");

                //var page1 = service.GetPage(pageId, PageLoadableChilds.None);
                //var page2 = service.GetPage(pageId, PageLoadableChilds.All);
                //var page3 = service.GetPage(pageId, PageLoadableChilds.Tags | PageLoadableChilds.Image);

//                Console.WriteLine("Pages: " + repository.AsQueryable<PageProperties>().Count());
//                Console.WriteLine("Pages with Fetch: " + repository.AsQueryable<PageProperties>().Fetch(p => p.Layout).Count());
//                Console.WriteLine("Pages with FetchMany: " + repository.AsQueryable<PageProperties>().FetchMany(p => p.PageTags).Count());
//                Console.WriteLine("Pages with Fetch Category + Order: " + repository.AsQueryable<PageProperties>().Fetch(p => p.Category).OrderBy(p => p.Title).Count());
//                Console.WriteLine("Pages with Fetch Layout + Order: " + repository.AsQueryable<PageProperties>().Fetch(p => p.Layout).OrderBy(p => p.Title).Count());
//                
//                Console.WriteLine("Pages with Where + Fetch Layout + Order: " + repository.AsQueryable<PageProperties>().Where(p => p.Id == Guid.Empty).Fetch(p => p.Layout).OrderBy(p => p.Title).Count());
//                Console.WriteLine("Pages with Fetch Layout + Where + Order: " + repository.AsQueryable<PageProperties>().Fetch(p => p.Layout).Where(p => p.Id == Guid.Empty).OrderBy(p => p.Title).Count());

                var test = service;


                
            });
        }
    }
}
