namespace BetterCms
{
    public interface ICmsDatabaseConfiguration
    {
        string ConnectionString { get; set; }

        string ConnectionStringName { get; set; }

        string SchemaName { get; set; }
    }
}