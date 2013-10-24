using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class CategoryMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Category_Successfully()
        {
            RunEntityMapTestsInTransaction(TestDataProvider.CreateNewCategory());            
        }
    }
}
