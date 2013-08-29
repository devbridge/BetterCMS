using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ModelTests.MapTests
{
    [TestFixture]
    public class BlogPostContentMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_BlogPostContent_Successfully()
        {
            var entity = TestDataProvider.CreateNewBlogPostContent();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}