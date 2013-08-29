using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class PageContentMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_PageContent_Successfully()
        {
            var entity = TestDataProvider.CreateNewPageContent();
            RunEntityMapTestsInTransaction(entity);            
        }

        [Test]
        public void Should_Insert_And_Retrieve_PageContent_PageContentOptions_Successfully()
        {
            var pageContent = TestDataProvider.CreateNewPageContent();

            var pageContentOptions = new[]
                {
                    TestDataProvider.CreateNewPageContentOption(pageContent),
                    TestDataProvider.CreateNewPageContentOption(pageContent)
                };
            pageContent.Options = pageContentOptions;

            SaveEntityAndRunAssertionsInTransaction(
                pageContent,
                result =>
                {
                    Assert.AreEqual(pageContent, result);
                    Assert.AreEqual(pageContentOptions.OrderBy(f => f.Id), result.Options.OrderBy(f => f.Id));
                });
        }
    }
}
