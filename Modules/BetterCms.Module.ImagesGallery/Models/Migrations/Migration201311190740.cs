using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.ImagesGallery.Models.Migrations
{
    /// <summary>
    /// Fixes broken custom options
    /// </summary>
    [Migration(201311190740)]
    public class Migration201311190740: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201311190740" /> class.
        /// </summary>
        public Migration201311190740()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrates UP.
        /// </summary>
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201311190740.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}