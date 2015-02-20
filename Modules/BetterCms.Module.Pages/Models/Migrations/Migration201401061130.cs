using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201401061130)]
    public class Migration201401061130: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201401061130"/> class.
        /// </summary>
        public Migration201401061130() : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Table("SitemapArchives")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("SitemapId").AsGuid().NotNullable()
                .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
                .WithColumn("ArchivedVersion").AsString(MaxLength.Max).NotNullable();

            Create
                .ForeignKey("FK_Cms_SitemapArchives_Cms_Sitemaps")
                .FromTable("SitemapArchives").InSchema(SchemaName).ForeignColumn("SitemapId")
                .ToTable("Sitemaps").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}