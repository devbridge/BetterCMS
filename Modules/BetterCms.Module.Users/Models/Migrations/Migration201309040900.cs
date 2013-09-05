using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201309040900)]
    public class Migration201309040900 : DefaultMigration
    {
        public Migration201309040900()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201309040900.sqlserver.sql");

            // TODO: Add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: Add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);    
        }
    }
}