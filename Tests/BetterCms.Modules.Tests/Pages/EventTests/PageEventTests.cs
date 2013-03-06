using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Module.MediaManager;
using BetterCms.Module.Pages.Api.Events;
using BetterCms.Module.Pages.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.EventTests
{
    [TestFixture]
    public class PageEventTests : DatabaseTestBase
    {
        private bool firedCreated;
        private bool firedDeleted;

        [Test]
        public void Should_Fire_Page_Created_Event()
        {
            firedCreated = false;
            
            PagesApiContext.Events.OnPageCreated(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(firedCreated);

            PagesApiContext.Events.PageCreated += delegate { firedCreated = true; };

            PagesApiContext.Events.OnPageCreated(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(firedCreated);
        }
        
        [Test]
        public void Should_Fire_Page_Deleted_Event()
        {
            firedDeleted = false;

            PagesApiContext.Events.OnPageDeleted(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(firedDeleted);

            PagesApiContext.Events.PageDeleted += delegate { firedDeleted = true; };

            PagesApiContext.Events.OnPageDeleted(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(firedDeleted);
        }
    }
}
