using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201403141036)]
    public class Migration201403141036: DefaultMigration
    {
        public Migration201403141036() : base(UsersModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201403141036.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}