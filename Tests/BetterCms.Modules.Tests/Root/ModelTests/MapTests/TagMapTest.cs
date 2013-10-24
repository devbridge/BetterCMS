using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class TagMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Tag_Successfully()
        {
            var entity = TestDataProvider.CreateNewTag();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
