using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201302211227)]
    public class Migration201302211227 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        public Migration201302211227()
            : base(RootModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            // Create new column in pages
            Create
                .Column("Status")
                .OnTable("Pages").InSchema(rootModuleSchemaName)
                .AsInt32().NotNullable().WithDefaultValue(3);
            
            // Drop column from page properties
            Delete
                .Column("IsPublished")
                .FromTable("Pages").InSchema(SchemaName);
        }

        public override void Down()
        {
            // Create new column in page properties
            Create
                .Column("IsPublished")
                .OnTable("Pages").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);

            // Drop column from pages
            Delete
                .Column("Status")
                .FromTable("Pages").InSchema(rootModuleSchemaName);
        }
    }
}