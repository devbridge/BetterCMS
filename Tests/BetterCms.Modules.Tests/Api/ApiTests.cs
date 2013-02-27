using System;
using System.Linq;

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
                var service = new DefaultContentApiService(repository);

                var pageId = new Guid("B0326B23-D0C0-4B4A-B7EE-A17200AE46BE");
//
                var contents1 = service.GetPageContents(pageId, c => c.Content is HtmlContent && ((HtmlContent)c.Content).Html.Contains("boo"));
//                var contents2 = service.GetContentHistory(contentId, c => c is HtmlContent && ((HtmlContent)c).Html.ToLower().Contains("draft"));
                //var contents3 = service.GetContentHistory(contentId, null, c => c.Name, true);

                //contentId = new Guid("DBC09596-572A-44D5-AC9C-A17200EBA112");
                //var results = service.GetContentHistory(contentId);


                var test = service;
            });
        }
    }
}
