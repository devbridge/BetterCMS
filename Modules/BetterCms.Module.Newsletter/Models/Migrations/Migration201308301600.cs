using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Newsletter.Models.Migrations
{
    [Migration(201308301600)]
    public class Migration201308301600: DefaultMigration
    {
        public Migration201308301600()
            : base(NewsletterModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201308301600.sqlserver.sql");

            // TODO: Add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: Add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);    
        }
    }
}