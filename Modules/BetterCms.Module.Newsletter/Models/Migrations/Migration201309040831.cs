using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Newsletter.Models.Migrations
{
    [Migration(201309040831)]
    public class Migration201309040831: DefaultMigration
    {
        public Migration201309040831()
            : base(NewsletterModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201309040831.sqlserver.sql");

            // TODO: Add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: Add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);    
        }
    }
}