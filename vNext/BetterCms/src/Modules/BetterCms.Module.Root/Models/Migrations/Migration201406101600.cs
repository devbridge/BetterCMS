using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201406101600)]
    public class Migration201406101600: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201406101600"/> class.
        /// </summary>
        public Migration201406101600()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201406101600.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}