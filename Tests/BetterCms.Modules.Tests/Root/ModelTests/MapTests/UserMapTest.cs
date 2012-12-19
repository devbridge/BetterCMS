using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class UserMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_User_Successfully()
        {
            var entity = TestDataProvider.CreateNewUser();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
