
using NUnit.Framework;

namespace BetterCms.Test.Module.Users.ModelTests.MapTests
{
    [TestFixture]
    public class PermissionMapTest : DatabaseTestBase
    {
        [Test]
        [Ignore]
        public void Should_Insert_And_Retrieve_Premissions_Successfully()
        {
            var entity = TestDataProvider.CreateNewPermission();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
