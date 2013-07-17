using BetterCms.Configuration;

namespace BetterCms
{
    public interface ICmsDatabaseConfiguration
    {
        string ConnectionString { get; set; }

        string ConnectionStringName { get; set; }

        string ConnectionProvider { get; set; }

        string SchemaName { get; set; }

        DatabaseType DatabaseType { get; set; }
    }
}