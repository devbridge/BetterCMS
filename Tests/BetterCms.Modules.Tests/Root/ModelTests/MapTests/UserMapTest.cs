using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class UserMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_User_Successfully()
        {
            var entity = TestDataProvider.CreateNewUser();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
