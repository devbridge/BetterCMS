using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201305271258)]
    public class Migration201305271258: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201305271258"/> class.
        /// </summary>
        public Migration201305271258()
            : base(BlogModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {            
            IfSqlServer().Execute.EmbeddedScript("Migration201305271258.sqlserver.sql");

            // TODO: Add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: Add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }       
    }
}