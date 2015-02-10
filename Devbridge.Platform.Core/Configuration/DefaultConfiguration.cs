using System.Configuration;

namespace Devbridge.Platform.Core.Configuration
{
    public class DefaultConfiguration :  ConfigurationSection, IConfiguration
    {
        private const string DatabaseNode = "database";

        [ConfigurationProperty(DatabaseNode, IsRequired = true)]
        public DatabaseConfigurationElement Database
        {
            get { return (DatabaseConfigurationElement)this[DatabaseNode]; }
            set { this[DatabaseNode] = value; }
        }

        IDatabaseConfiguration IConfiguration.Database
        {
            get { return Database; }
        }
    }
}
