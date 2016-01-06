using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Categories: created parent category id and macro.
    /// </summary>
    [Migration(201502061445)]
    public class Migration201502061445 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201502041430"/> class.
        /// </summary>
        public Migration201502061445()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                .Table("WidgetCategories")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("WidgetId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_WidgetCategories_WidgetId_Cms_Widget_Id")
                .FromTable("WidgetCategories").InSchema(SchemaName).ForeignColumn("WidgetId")
                .ToTable("Widgets").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_WidgetCategories_CategoryId_Cms_Category_Id")
                .FromTable("WidgetCategories").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_WidgetCategories_WidgetId")
                .OnTable("WidgetCategories").InSchema(SchemaName).OnColumn("WidgetId");

            Create
                .Index("IX_Cms_WidgetCategories_CategoryId")
                .OnTable("WidgetCategories").InSchema(SchemaName).OnColumn("CategoryId");


            IfSqlServer().Execute.EmbeddedScript("Migration201502061445.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);

        }
    }
}