using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class HtmlContentHistoryMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_HtmlContentHistory_Successfully()
        {
            var htmlContent = TestDataProvider.CreateNewHtmlContentHistory();
            RunEntityMapTestsInTransaction(htmlContent);  
        }
    }
}
