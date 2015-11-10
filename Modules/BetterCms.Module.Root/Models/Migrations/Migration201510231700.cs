using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201510231700)]
    public class Migration201510231700: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201406101600"/> class.
        /// </summary>
        public Migration201510231700()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201510231700.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}