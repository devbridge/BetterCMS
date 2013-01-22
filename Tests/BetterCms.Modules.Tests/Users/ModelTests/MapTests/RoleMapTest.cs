
using NUnit.Framework;

namespace BetterCms.Test.Module.Users.ModelTests.MapTests
{
    [TestFixture]
    public class RoleMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrive_Role_Successfully()
        {
            var role = TestDataProvider.CreateNewRole();
            RunEntityMapTestsInTransaction(role);
        }
    }
}
