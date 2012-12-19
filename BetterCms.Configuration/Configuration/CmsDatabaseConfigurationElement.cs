using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class CmsDatabaseConfigurationElement : ConfigurationElement, ICmsDatabaseConfiguration
    {
        private const string SchemaNameAttribute = "schemaName";
        private const string ConnectionStringNameAttribute = "connectionStringName";
        private const string ConnectionStringAttribute = "connectionString";

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
    }
}