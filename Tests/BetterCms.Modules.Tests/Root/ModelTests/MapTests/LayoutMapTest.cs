using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class LayoutMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Layout_Successfully()
        {
            var layout = TestDataProvider.CreateNewLayout();
            RunEntityMapTestsInTransaction(layout);
        }

        [Test]
        public void Should_Insert_And_Retrieve_Layout_Pages_Successfully()
        {
            var layout = TestDataProvider.CreateNewLayout();
            var pages = new[]
                {
                    TestDataProvider.CreateNewPage(layout),
                    TestDataProvider.CreateNewPage(layout),
                    TestDataProvider.CreateNewPage(layout)                    
                };
            layout.Pages = pages;

            SaveEntityAndRunAssertionsInTransaction(
                layout,
                result =>
                    {
                        Assert.AreEqual(layout, result);
                        Assert.AreEqual(pages.OrderBy(f => f.Id), result.Pages.OrderBy(f => f.Id));
                    });
        }

        [Test]
        public void Should_Insert_And_Retrieve_Layout_LayoutRegions_Successfully()
        {
            var layout = TestDataProvider.CreateNewLayout();
            var layoutRegions = new[]
                {
                    TestDataProvider.CreateNewLayoutRegion(layout),
                    TestDataProvider.CreateNewLayoutRegion(layout),
                    TestDataProvider.CreateNewLayoutRegion(layout)                    
                };
            layout.LayoutRegions = layoutRegions;

            SaveEntityAndRunAssertionsInTransaction(
                layout,
                result =>
                {
                    Assert.AreEqual(layout, result);
                    Assert.AreEqual(layoutRegions.OrderBy(f => f.Id), result.LayoutRegions.OrderBy(f => f.Id));
                });
        }
    }
}
