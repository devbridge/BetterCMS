using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ModelTests.MapTests
{
    [TestFixture]
    public class OptionMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_BlogOption_Successfully()
        {
            var entity = TestDataProvider.CreateNewBlogOption();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}