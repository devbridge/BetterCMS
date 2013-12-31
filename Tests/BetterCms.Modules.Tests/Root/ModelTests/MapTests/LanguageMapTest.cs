using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class LanguageMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Language_Successfully()
        {
            RunEntityMapTestsInTransaction(TestDataProvider.CreateNewLanguage());            
        }
    }
}
