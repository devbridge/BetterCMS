using Devbridge.Platform.Core.Configuration;
using Devbridge.Platform.Core.Web.Configuration;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Configuration
{
    [TestFixture]
    public class DefaultWebConfigurationLoaderTests : TestBase
    {
        [Test]
        public void ShoudlLoad_DefaultWebConfigurationSection_Successfully()
        {
            var service = new DefaultConfigurationLoader();
            var configuration = service.LoadConfig<DefaultWebConfigurationSection>();

            Assert.IsNotNull(configuration);
            Assert.IsNotNull(configuration.Database);

            Assert.AreEqual(configuration.Database.DatabaseType, DatabaseType.MsSql2008);
            Assert.AreEqual(configuration.Database.ConnectionStringName, "PlatformTests");
            Assert.AreEqual(configuration.Database.SchemaName, "dbo");

            Assert.AreEqual(configuration.WebSiteUrl, "http://devbridge.platform.core.tests");
        }

        [Test]
        public void ShoudlTryLoad_DefaultWebConfigurationSection_Successfully()
        {
            var service = new DefaultConfigurationLoader();
            var configuration = service.TryLoadConfig<DefaultWebConfigurationSection>();

            Assert.IsNotNull(configuration);
            Assert.IsNotNull(configuration.Database);

            Assert.AreEqual(configuration.Database.DatabaseType, DatabaseType.MsSql2008);
            Assert.AreEqual(configuration.Database.ConnectionStringName, "PlatformTests");
            Assert.AreEqual(configuration.Database.SchemaName, "dbo");

            Assert.AreEqual(configuration.WebSiteUrl, "http://devbridge.platform.core.tests");
        }
    }
}
