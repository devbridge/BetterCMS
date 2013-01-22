
using NUnit.Framework;

namespace BetterCms.Test.Module.Users.ModelTests.MapTests
{
    [TestFixture]
    public class PremissionMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Premissions_Successfully()
        {
            var entity = TestDataProvider.CreateNewPremission();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
