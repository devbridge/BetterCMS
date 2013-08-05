using NUnit.Framework;

namespace BetterCms.Test.Module.AccessControl.ModelTests.MapTests
{
    public class UserAccessMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_UserAccess_Successfully()
        {
            var content = TestDataProvider.CreateNewUserAccess();
            RunEntityMapTestsInTransaction(content);
        }
    }
}