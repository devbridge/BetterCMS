using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201302211227)]
    public class Migration201302211227: DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201302211227"/> class.
        /// </summary>
        public Migration201302211227()
            : base(RootModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
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
    }
}