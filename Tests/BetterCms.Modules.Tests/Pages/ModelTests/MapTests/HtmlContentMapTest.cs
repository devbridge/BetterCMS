using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class HtmlContentMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_HtmlContent_Successfully()
        {
            var htmlContent = TestDataProvider.CreateNewHtmlContent(200);
            RunEntityMapTestsInTransaction(htmlContent); 
        }
    }
}
