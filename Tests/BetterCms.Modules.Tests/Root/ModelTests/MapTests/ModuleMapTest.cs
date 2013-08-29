using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class ModuleMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Module_Successfully()
        {
            var entity = TestDataProvider.CreateNewModule();
            RunEntityMapTestsInTransaction(entity);            
        }
    }
}
