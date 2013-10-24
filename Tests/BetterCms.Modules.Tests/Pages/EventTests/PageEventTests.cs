using BetterCms.Module.Pages.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.EventTests
{
    [TestFixture]
    public class PageEventTests : IntegrationTestBase
    {
        private bool firedCreated;
        private bool firedDeleted;

        [Test]
        public void Should_Fire_Page_Created_Event()
        {
            firedCreated = false;
            
            Events.PageEvents.Instance.OnPageCreated(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(firedCreated);

            Events.PageEvents.Instance.PageCreated += delegate { firedCreated = true; };
            
            Events.PageEvents.Instance.OnPageCreated(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(firedCreated);
        }
        
        [Test]
        public void Should_Fire_Page_Deleted_Event()
        {
            firedDeleted = false;

            Events.PageEvents.Instance.OnPageDeleted(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(firedDeleted);

            Events.PageEvents.Instance.PageDeleted += delegate { firedDeleted = true; };

            Events.PageEvents.Instance.OnPageDeleted(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(firedDeleted);
        }
    }
}
