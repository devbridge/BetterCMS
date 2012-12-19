using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class PageCategoryMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_PageCategory_Successfully()
        {
            var entity = TestDataProvider.CreateNewPageCategory();
            RunEntityMapTestsInTransaction(
                entity,
                result =>
                    {
                        Assert.AreEqual(entity.Page.PageUrl, result.Page.PageUrl);
                        Assert.AreEqual(entity.Category.Name, result.Category.Name);
                    });


        }
    }
}
