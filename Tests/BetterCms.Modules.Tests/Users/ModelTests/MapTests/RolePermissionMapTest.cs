
using NUnit.Framework;

namespace BetterCms.Test.Module.Users.ModelTests.MapTests
{
    [TestFixture]
    public class RolePermissionMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_RolePremission_Successfully()
        {
            var entity = TestDataProvider.CreateNewRolePermission();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
