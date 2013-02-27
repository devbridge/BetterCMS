using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.DataServices;
using BetterCms.Module.MediaManager.DataServices;
using BetterCms.Module.MediaManager.Models;
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

                var pageId = new Guid("B0326B23-D0C0-4B4A-B7EE-A17200AE46BE");
//
//                var contents1 = service.GetPageContents(pageId, p => ((PageProperties)p.Page).CustomCss.Transliterate(true) == "agfa");
//                var contents2 = service.GetRegionContents(pageId, contents1[0].Region.Id);
//                var contents3 = service.GetRegionContents(pageId, contents1[0].Region.RegionIdentifier);
//                var content = service.GetContent(contents1[0].Content.Id);
//                var pageContent = service.GetPageContent(contents1[0].Id);

                //var result = service.GetFile(folder.Id);







//                var test = contents1;
            });
        }
    }
}
