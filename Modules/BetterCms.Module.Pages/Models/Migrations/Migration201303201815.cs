using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Migration for IsInSitemap.
    /// </summary>
    [Migration(201303201815)]
    public class Migration201303201815 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201303050900"/> class.
        /// </summary>
        public Migration201303201815()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("EditInSourceMode")
                .OnTable("HtmlContents").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);
            
            Create
                .Column("EditInSourceMode")
                .OnTable("HtmlContentWidgets").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}