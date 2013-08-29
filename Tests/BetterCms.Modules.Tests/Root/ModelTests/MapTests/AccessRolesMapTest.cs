using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    public class AccessRolesMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_AccessRule_Successfully()
        {
            var content = TestDataProvider.CreateNewAccessRule();
            RunEntityMapTestsInTransaction(content);
        }
    }
}