
using NUnit.Framework;

namespace BetterCms.Test.Module.Users.ModelTests.MapTests
{
    [TestFixture]
    public class RolePremissionMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_RolePremission_Successfully()
        {
            var entity = TestDataProvider.CreateNewRolePremission();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
