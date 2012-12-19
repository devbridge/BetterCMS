using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class SiteSettingMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_SiteSettings_Successfully()
        {
            var entity = TestDataProvider.CreateNewSiteSettings();
            RunEntityMapTestsInTransaction(entity);
        }
    }
}
