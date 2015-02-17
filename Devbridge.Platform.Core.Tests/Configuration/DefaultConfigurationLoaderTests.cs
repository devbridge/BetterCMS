using Devbridge.Platform.Core.Configuration;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Configuration
{
    [TestFixture]
    public class DefaultConfigurationLoaderTests : TestBase
    {
        [Test]
        public void ShoudlLoad_DefaultConfigurationSection_Successfully()
        {
            var service = new DefaultConfigurationLoader();
            var configuration = service.LoadConfig<DefaultConfigurationSection>();

            Assert.IsNotNull(configuration);
            Assert.IsNotNull(configuration.Database);
            
            Assert.AreEqual(configuration.Database.DatabaseType, DatabaseType.MsSql2008);
            Assert.AreEqual(configuration.Database.ConnectionStringName, "PlatformTests");
            Assert.AreEqual(configuration.Database.SchemaName, "dbo");
        }
        
        [Test]
        public void ShoudlTryLoad_DefaultConfigurationSection_Successfully()
        {
            var service = new DefaultConfigurationLoader();
            var configuration = service.TryLoadConfig<DefaultConfigurationSection>();

            Assert.IsNotNull(configuration);
            Assert.IsNotNull(configuration.Database);
            
            Assert.AreEqual(configuration.Database.DatabaseType, DatabaseType.MsSql2008);
            Assert.AreEqual(configuration.Database.ConnectionStringName, "PlatformTests");
            Assert.AreEqual(configuration.Database.SchemaName, "dbo");
        }
    }
}
