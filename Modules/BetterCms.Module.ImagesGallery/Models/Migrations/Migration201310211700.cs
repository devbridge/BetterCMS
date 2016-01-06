using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.ImagesGallery.Models.Migrations
{
    /// <summary>
    /// Creates category for image gallery widgets and assigns it to widgets
    /// </summary>
    [Migration(201310211700)]
    public class Migration201310211700: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310211700" /> class.
        /// </summary>
        public Migration201310211700()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrates UP.
        /// </summary>
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201310211700.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}