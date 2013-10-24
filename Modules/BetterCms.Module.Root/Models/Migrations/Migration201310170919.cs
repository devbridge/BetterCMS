using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201310170919)]
    public class Migration201310170919 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310170919"/> class.
        /// </summary>
        public Migration201310170919()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Delete
                .UniqueConstraint("UX_Cms_Pages_PageUrl")
                .FromTable("Pages").InSchema(SchemaName);

            Delete
                .Index("IX_Cms_Pages_PageUrl")
                .OnTable("Pages").InSchema(SchemaName);

            Delete
                .UniqueConstraint("UX_Cms_Pages_PageUrlLowerTrimmed")
                .FromTable("Pages").InSchema(SchemaName);

            Delete
                .Index("IX_Cms_Pages_PageUrlLowerTrimmed")
                .OnTable("Pages").InSchema(SchemaName);

            Delete
                .Column("PageUrlLowerTrimmed")
                .FromTable("Pages").InSchema(SchemaName);
                
            Alter
                .Table("Pages").InSchema(SchemaName)
                .AddColumn("PageUrlHash").AsAnsiString(MaxLength.UrlHash).Nullable();

            IfSqlServer().Execute.EmbeddedScript("Migration201310170919.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);

            Alter
                .Table("Pages").InSchema(SchemaName)
                .AlterColumn("PageUrlHash").AsAnsiString(MaxLength.UrlHash).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Pages_PageUrlHash")
                .OnTable("Pages").WithSchema(SchemaName)
                .Columns(new[] { "PageUrlHash", "DeletedOn" });

            Create
                .Index("IX_Cms_Pages_PageUrlHash")
                .OnTable("Pages").InSchema(SchemaName)
                .OnColumn("PageUrlHash").Ascending();
        }       
    }
}