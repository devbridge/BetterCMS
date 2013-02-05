using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(201302050830)]
    public class Migration201302050830 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        public Migration201302050830()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            // Create new column in pages
            Create
                .Column("IsPublic")
                .OnTable("Pages").InSchema(rootModuleSchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(true);
            
            // Drop column from page properties
            Delete
                .Column("IsPublic")
                .FromTable("Pages").InSchema(SchemaName);
        }

        public override void Down()
        {
            // Create new column in page properties
            Create
                .Column("IsPublic")
                .OnTable("Pages").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(true);

            // Drop column from pages
            Delete
                .Column("IsPublic")
                .FromTable("Pages").InSchema(rootModuleSchemaName);
        }
    }
}