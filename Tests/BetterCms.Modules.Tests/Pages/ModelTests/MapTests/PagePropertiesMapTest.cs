using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class PagePropertiesMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_PageProperties_Successfully()
        {
            var entity = TestDataProvider.CreateNewPageProperties();
            RunEntityMapTestsInTransaction(entity);
        }

        [Test]
        public void Should_Insert_And_Retrieve_PageProperties_PageTags_Successfully()
        {
            var page = TestDataProvider.CreateNewPageProperties();

            var pageTags = new[]
                {
                    TestDataProvider.CreateNewPageTag(page),
                    TestDataProvider.CreateNewPageTag(page),
                    TestDataProvider.CreateNewPageTag(page)
                };

            page.PageTags = pageTags;

            SaveEntityAndRunAssertionsInTransaction(
                page,
                result =>
                {
                    Assert.AreEqual(page, result);
                    Assert.AreEqual(pageTags.OrderBy(f => f.Id), result.PageTags.OrderBy(f => f.Id));
                });
        }

        [Test]
        public void Should_Insert_And_Retrieve_PageProperties_PageCategories_Successfully()
        {
            var page = TestDataProvider.CreateNewPageProperties();

            var pageCategories = new[]
                {
                    TestDataProvider.CreateNewPageCategory(page),
                    TestDataProvider.CreateNewPageCategory(page),
                    TestDataProvider.CreateNewPageCategory(page)
                };

            page.PageCategories = pageCategories;

            SaveEntityAndRunAssertionsInTransaction(
                page,
                result =>
                {
                    Assert.AreEqual(page, result);
                    Assert.AreEqual(pageCategories.OrderBy(f => f.Id), result.PageCategories.OrderBy(f => f.Id));
                });
        }
    }
}
