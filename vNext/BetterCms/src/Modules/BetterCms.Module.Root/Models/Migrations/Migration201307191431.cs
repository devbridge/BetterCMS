using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201307191431)]
    public class Migration201307191431: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201307191431" /> class.
        /// </summary>
        public Migration201307191431()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                .Column("PageUrlLowerTrimmed")
                .OnTable("Pages")
                .InSchema(SchemaName)
                .AsAnsiString(MaxLength.Url)
                .Nullable();

            IfSqlServer().Execute.EmbeddedScript("Migration201307191431.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);

            Alter
                .Table("Pages")
                .InSchema(SchemaName)
                .AlterColumn("PageUrlLowerTrimmed")
                .AsAnsiString(MaxLength.Url)
                .NotNullable();
        }
    }
}