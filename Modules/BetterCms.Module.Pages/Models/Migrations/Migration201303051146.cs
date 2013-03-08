using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Migration for IsInSitemap.
    /// </summary>
    [Migration(201303051146)]
    public class Migration201303051146 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201303050900"/> class.
        /// </summary>
        public Migration201303051146()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            Delete
                .Column("IsInSitemap")
                .FromTable("Pages").InSchema(SchemaName);

            Create
                .Column("NodeCountInSitemap")
                .OnTable("Pages").InSchema(SchemaName)
                .AsInt32().NotNullable().WithDefaultValue(0);
        }

        /// <summary>
        /// Migrate down.
        /// </summary>
        public override void Down()
        {
            Delete
                .Column("NodeCountInSitemap")
                .FromTable("Pages").InSchema(SchemaName);

            Create
                .Column("IsInSitemap")
                .OnTable("Pages").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}