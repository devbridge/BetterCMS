using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201401211353)]
    public class Migration201401211353: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201401211353"/> class.
        /// </summary>
        public Migration201401211353()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("Macro")
                .OnTable("SitemapNodes").InSchema(SchemaName)
                .AsString(MaxLength.Text).Nullable();

            Create
                .Column("Macro")
                .OnTable("SitemapNodeTranslations").InSchema(SchemaName)
                .AsString(MaxLength.Text).Nullable();
        }
    }
}