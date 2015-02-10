namespace Devbridge.Platform.Core.Configuration
{
    public interface IDatabaseConfiguration
    {
        string ConnectionString { get; set; }

        string ConnectionStringName { get; set; }

        string ConnectionProvider { get; set; }

        string SchemaName { get; set; }

        DatabaseType DatabaseType { get; set; }
    }
}