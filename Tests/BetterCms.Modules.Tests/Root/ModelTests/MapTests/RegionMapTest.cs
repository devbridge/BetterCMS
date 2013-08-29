using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class RegionMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Region_Successfully()
        {
            var entity = TestDataProvider.CreateNewRegion();
            RunEntityMapTestsInTransaction(entity);
        }

        [Test]
        public void Should_Insert_And_Retrieve_Region_PageContentOptions_Successfully()
        {
            var region = TestDataProvider.CreateNewRegion();

            var pageContents = new[]
                {
                    TestDataProvider.CreateNewPageContent(null, null, region)
                };

            region.PageContents = pageContents;

            SaveEntityAndRunAssertionsInTransaction(
                region,
                result =>
                {
                    Assert.AreEqual(region, result);
                    Assert.AreEqual(pageContents.OrderBy(f => f.Id), result.PageContents.OrderBy(f => f.Id));
                });
        }
        
        [Test]
        public void Should_Insert_And_Retrieve_Region_LayoutRegions_Successfully()
        {
            var region = TestDataProvider.CreateNewRegion();

            var layoutRegions = new[]
                {
                    TestDataProvider.CreateNewLayoutRegion(null, region),
                    TestDataProvider.CreateNewLayoutRegion(null, region)
                };

            region.LayoutRegion = layoutRegions;

            SaveEntityAndRunAssertionsInTransaction(
                region,
                result =>
                {
                    Assert.AreEqual(region, result);
                    Assert.AreEqual(layoutRegions.OrderBy(f => f.Id), result.LayoutRegion.OrderBy(f => f.Id));
                });
        }
    }
}
