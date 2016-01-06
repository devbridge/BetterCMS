using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Installation.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201311181011)]
    public class Migration201311181011: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201311181011"/> class.
        /// </summary>
        public Migration201311181011()
            : base(InstallationModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201311181011.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}