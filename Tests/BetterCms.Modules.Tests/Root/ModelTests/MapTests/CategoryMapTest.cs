using System.Linq;
using System.Runtime.InteropServices;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class CategoryMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Category_Successfully()
        {
            var categoryTree = TestDataProvider.CreateNewCategoryTree();
            RunEntityMapTestsInTransaction(TestDataProvider.CreateNewCategory(categoryTree));            
        }
    }
}
