using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201406101545)]
    public class Migration201406101545 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201406101545"/> class.
        /// </summary>
        public Migration201406101545()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201406101545.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}