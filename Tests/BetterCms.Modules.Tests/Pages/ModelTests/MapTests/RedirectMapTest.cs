using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class RedirectMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Redirect_Successfully()
        {
            var content = TestDataProvider.CreateNewRedirect();
            RunEntityMapTestsInTransaction(content); 
        }
    }
}
