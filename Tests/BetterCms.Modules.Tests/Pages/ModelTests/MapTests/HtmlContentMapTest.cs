using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class HtmlContentMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_HtmlContent()
        {
            var htmlContent = TestDataProvider.CreateNewHtmlContent(200);

            RunEntityMapTestsInTransaction(htmlContent);
        }
    }
}
