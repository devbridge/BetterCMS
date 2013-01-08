using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ModelTests.MapTests
{
    [TestFixture]
    public class MediaFileMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_BlogPost_Successfully()
        {
            var entity = TestDataProvider.CreateNewBlogPost();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}