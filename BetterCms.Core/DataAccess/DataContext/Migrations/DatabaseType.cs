namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    /// <summary>
    /// Available database types to migrate.
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Microsoft SQL Server database.
        /// </summary>
        SqlServer,

        /// <summary>
        /// Microsoft SQL Azure cloud database.
        /// </summary>
        SqlAzure,

        /// <summary>
        /// PostgreSQL database.
        /// </summary>
        PostgreSQL,

        /// <summary>
        /// Oracle database.
        /// </summary>
        Oracle
    }
}
