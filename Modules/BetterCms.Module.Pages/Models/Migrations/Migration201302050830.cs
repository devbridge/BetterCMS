using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201302050830)]
    public class Migration201302050830: DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201302050830"/> class.
        /// </summary>
        public Migration201302050830()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
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
    }
}