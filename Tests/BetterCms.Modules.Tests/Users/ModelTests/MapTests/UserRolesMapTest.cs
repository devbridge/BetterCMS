using NUnit.Framework;

namespace BetterCms.Test.Module.Users.ModelTests.MapTests
{
    [TestFixture]
    class UserRolesMapTest : IntegrationTestBase
    {
        [Test]
        [Ignore]
        public void Should_Insert_And_Retrieve_UserRoles_Successfully()
        {
            var entity = TestDataProvider.CreateNewUserRoles();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
