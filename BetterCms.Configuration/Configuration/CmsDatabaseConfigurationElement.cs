using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class CmsDatabaseConfigurationElement : ConfigurationElement, ICmsDatabaseConfiguration
    {
        private string connectionProvider;

        private const string SchemaNameAttribute = "schemaName";
        private const string ConnectionStringNameAttribute = "connectionStringName";
        private const string ConnectionStringAttribute = "connectionString";
        private const string ConnectionProviderAttribute = "connectionProvider";
        private const string DatabaseTypeAttribute = "databaseType";
        
        [ConfigurationProperty(SchemaNameAttribute, DefaultValue = "dbo", IsRequired = false)]
        public string SchemaName
        {
            get { return Convert.ToString(this[SchemaNameAttribute]); }
            set { this[SchemaNameAttribute] = value; }
        }

        [ConfigurationProperty(ConnectionStringAttribute, IsRequired = false)]
        public string ConnectionString
        {
            get { return Convert.ToString(this[ConnectionStringAttribute]); }
            set { this[ConnectionStringAttribute] = value; }
        }

        [ConfigurationProperty(ConnectionStringNameAttribute, IsRequired = false)]
        public string ConnectionStringName
        {
            get { return Convert.ToString(this[ConnectionStringNameAttribute]); }
            set { this[ConnectionStringNameAttribute] = value; }
        }

        [ConfigurationProperty(ConnectionProviderAttribute, IsRequired = false)]
        public string ConnectionProvider
        {
            get { return Convert.ToString(this[ConnectionProviderAttribute]); }
            set { this[ConnectionProviderAttribute] = value; }
        }

        [ConfigurationProperty(DatabaseTypeAttribute, IsRequired = false, DefaultValue = DatabaseType.MsSql2008)]
        public DatabaseType DatabaseType
        {
            get { return (DatabaseType)this[DatabaseTypeAttribute]; }
            set { this[DatabaseTypeAttribute] = value; }
        }
    }
}