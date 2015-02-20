using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201401091655)]
    public class Migration201401091655: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201401091655"/> class.
        /// </summary>
        public Migration201401091655()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Table("SitemapNodeTranslations")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("NodeId").AsGuid().NotNullable()
                .WithColumn("LanguageId").AsGuid().NotNullable()
                .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
                .WithColumn("UsePageTitleAsNodeTitle").AsBoolean().NotNullable()
                .WithColumn("Url").AsString(MaxLength.Url).Nullable()
                .WithColumn("UrlHash").AsAnsiString(MaxLength.UrlHash).Nullable();

            Create
                .ForeignKey("FK_Cms_SitemapNodeTranslations_Cms_Sitemaps")
                .FromTable("SitemapNodeTranslations").InSchema(SchemaName).ForeignColumn("NodeId")
                .ToTable("SitemapNodes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_SitemapNodeTranslations_UrlHash")
                .OnTable("SitemapNodeTranslations").InSchema(SchemaName)
                .OnColumn("UrlHash").Ascending();
        }
    }
}