using BetterCms.Module.Pages.Events;
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
            
            PagesEvents.Instance.OnPageCreated(null, new PageCreatedEventArgs(new PageProperties()));
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(firedCreated);

            PagesEvents.Instance.PageCreated += delegate { firedCreated = true; };

            PagesEvents.Instance.OnPageCreated(null, new PageCreatedEventArgs(new PageProperties()));
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(firedCreated);
        }
        
        [Test]
        public void Should_Fire_Page_Deleted_Event()
        {
            firedDeleted = false;

            PagesEvents.Instance.OnPageDeleted(null, new PageDeletedEventArgs(new PageProperties()));
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(firedDeleted);

            PagesEvents.Instance.PageDeleted += delegate { firedDeleted = true; };

            PagesEvents.Instance.OnPageDeleted(null, new PageDeletedEventArgs(new PageProperties()));
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(firedDeleted);
        }
    }
}
