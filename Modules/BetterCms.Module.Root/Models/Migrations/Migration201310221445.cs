using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201310221445)]
    public class Migration201310221445: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310221445"/> class.
        /// </summary>
        public Migration201310221445()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201310221445.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }       
    }
}