using NUnit.Framework;

namespace BetterCms.Test.Module.Users.ModelTests.MapTests
{
    [TestFixture]
    class UserMapTest : DatabaseTestBase
    {
        [Test]
        [Ignore]
        public void Should_Insert_And_Retrieve_User_Successfully()
        {
            var entity = TestDataProvider.CreateNewUser();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
