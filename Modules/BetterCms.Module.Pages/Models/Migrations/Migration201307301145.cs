using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201307301145)]
    public class Migration201307301145: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201307301145"/> class.
        /// </summary>
        public Migration201307301145()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Delete
                .Column("CanonicalUrl")
                .FromTable("Pages")
                .InSchema(SchemaName);
        }
    }
}