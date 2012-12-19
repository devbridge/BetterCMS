using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class ContentMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Content_Successfully()
        {
            var content = TestDataProvider.CreateNewContent();
            RunEntityMapTestsInTransaction(content);            
        }

        [Test]
        public void Should_Insert_And_Retrieve_Content_PageContents_Successfully()
        {
            var content = TestDataProvider.CreateNewContent();
            var pageContents = new[]
                {
                    TestDataProvider.CreateNewPageContent(content),
                    TestDataProvider.CreateNewPageContent(content)
                };
            content.PageContents = pageContents;

            SaveEntityAndRunAssertionsInTransaction(
                content,
                result =>
                    {
                        Assert.AreEqual(content, result);
                        Assert.AreEqual(pageContents.OrderBy(f => f.Id), result.PageContents.OrderBy(f => f.Id));
                    });
        }

        [Test]
        public void Should_Insert_And_Retrieve_Content_ContentOptions_Successfully()
        {
            var content = TestDataProvider.CreateNewContent();
            var contentOptions = new[]
                {
                    TestDataProvider.CreateNewContentOption(content),
                    TestDataProvider.CreateNewContentOption(content)
                };
            content.ContentOptions = contentOptions;

            SaveEntityAndRunAssertionsInTransaction(
                content,
                result =>
                    {
                        Assert.AreEqual(content, result);
                        Assert.AreEqual(contentOptions.OrderBy(f => f.Id), result.ContentOptions.OrderBy(f => f.Id));
                    });
        }
    }
}
