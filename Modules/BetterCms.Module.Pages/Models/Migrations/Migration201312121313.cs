using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201312121313)]
    public class Migration201312121313: DefaultMigration
    {
        /// <summary>
        /// The default sitemap identifier.
        /// </summary>
        private const string DefaultSitemapId = "17ABFEE9-5AE6-470C-92E1-C2905036574B";

        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201312121313"/> class.
        /// </summary>
        public Migration201312121313() : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateSitemapsTable();
            CreateSitemapTagsTable();
            CreateSitemapAccessRulesTable();

            CreateDefaultSitemap();

            UpdateSitemapNodesTable();
            UpdatePagesTable();
        }

        /// <summary>
        /// Creates the sitemaps table.
        /// </summary>
        private void CreateSitemapsTable()
        {
            Create
                .Table("Sitemaps")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Title").AsString(MaxLength.Name).NotNullable();
        }

        /// <summary>
        /// Creates the sitemap tags table.
        /// </summary>
        private void CreateSitemapTagsTable()
        {
            Create
                .Table("SitemapTags")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("SitemapId").AsGuid().NotNullable()
                .WithColumn("TagId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_SitemapTags_Cms_Sitemaps")
                .FromTable("SitemapTags").InSchema(SchemaName).ForeignColumn("SitemapId")
                .ToTable("Sitemaps").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_SitemapTags_Cms_Tags")
                .FromTable("SitemapTags").InSchema(SchemaName).ForeignColumn("TagId")
                .ToTable("Tags").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the sitemap access rules table.
        /// </summary>
        private void CreateSitemapAccessRulesTable()
        {
            Create
                .Table("SitemapAccessRules")
                .InSchema(SchemaName)
                .WithColumn("SitemapId").AsGuid().NotNullable()
                .WithColumn("AccessRuleId").AsGuid().NotNullable();

            Create
                .PrimaryKey("PK_Cms_SitemapAccessRules")
                .OnTable("SitemapAccessRules").WithSchema(SchemaName)
                .Columns(new[] { "SitemapId", "AccessRuleId" });

            Create
                .ForeignKey("FK_Cms_SitemapAccessRules_SitemapId_Cms_Sitemap_Id")
                .FromTable("SitemapAccessRules").InSchema(SchemaName).ForeignColumn("SitemapId")
                .ToTable("Sitemaps").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_SitemapAccessRules_AccessRuleId_Cms_AccessRules_Id")
                .FromTable("SitemapAccessRules").InSchema(SchemaName).ForeignColumn("AccessRuleId")
                .ToTable("AccessRules").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_SitemapAccessRules_SitemapId")
                .OnTable("SitemapAccessRules").InSchema(SchemaName).OnColumn("SitemapId");

            Create
                .Index("IX_Cms_SitemapAccessRules_AccessRuleId")
                .OnTable("SitemapAccessRules").InSchema(SchemaName).OnColumn("AccessRuleId");
        }

        /// <summary>
        /// Creates the default sitemap.
        /// </summary>
        private void CreateDefaultSitemap()
        {
            Insert.IntoTable("Sitemaps")
                  .InSchema(SchemaName)
                  .Row(
                      new
                      {
                          Id = DefaultSitemapId,
                          Version = 1,
                          IsDeleted = false,
                          CreatedOn = DateTime.Now,
                          CreatedByUser = "Better CMS",
                          ModifiedOn = DateTime.Now,
                          ModifiedByUser = "Better CMS",
                          Title = "Default Site Map"
                      });
        }

        /// <summary>
        /// Updates the sitemap nodes table.
        /// </summary>
        private void UpdateSitemapNodesTable()
        {
            CorrelateWithSitemap();
            CorrelateWithPage();
            CreateUrlHashColumn();
        }

        private void CreateUrlHashColumn()
        {
            Alter
                .Table("SitemapNodes").InSchema(SchemaName)
                .AddColumn("UrlHash").AsAnsiString(MaxLength.UrlHash).Nullable();

            IfSqlServer().Execute.EmbeddedScript("Migration201312121313.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);

            Create
                .Index("IX_Cms_SitemapNodes_UrlHash")
                .OnTable("SitemapNodes").InSchema(SchemaName)
                .OnColumn("UrlHash").Ascending();
        }

        /// <summary>
        /// Correlates the with sitemap.
        /// </summary>
        private void CorrelateWithSitemap()
        {
            Create
                .Column("SitemapId")
                .OnTable("SitemapNodes").InSchema(SchemaName)
                .AsGuid().Nullable();

            Update
                .Table("SitemapNodes").InSchema(SchemaName)
                .Set(new { SitemapId = DefaultSitemapId })
                .AllRows();

            Alter
                .Table("SitemapNodes")
                .InSchema(SchemaName)
                .AlterColumn("SitemapId")
                .AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_SitemapNodes_SitemapId_Cms_Sitemap_Id")
                .FromTable("SitemapNodes").InSchema(SchemaName).ForeignColumn("SitemapId")
                .ToTable("Sitemaps").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_SitemapNodes_SitemapId")
                .OnTable("SitemapNodes").InSchema(SchemaName).OnColumn("SitemapId");

            Create
                .Column("UsePageTitleAsNodeTitle")
                .OnTable("SitemapNodes").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);

            Alter
                .Table("SitemapNodes")
                .InSchema(SchemaName)
                .AlterColumn("UsePageTitleAsNodeTitle")
                .AsBoolean().NotNullable();
        }

        /// <summary>
        /// Correlates the with page.
        /// </summary>
        private void CorrelateWithPage()
        {
            Create
                .Column("PageId")
                .OnTable("SitemapNodes").InSchema(SchemaName)
                .AsGuid().Nullable();

            Alter
                .Table("SitemapNodes").InSchema(SchemaName)
                .AlterColumn("Url").AsString(MaxLength.Url).Nullable();

            Create
                .ForeignKey("FK_Cms_SitemapNodes_SitemapId_Cms_Page_Id")
                .FromTable("SitemapNodes").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_SitemapNodes_PageId")
                .OnTable("SitemapNodes").InSchema(SchemaName).OnColumn("PageId");
        }

        /// <summary>
        /// Updates the pages table.
        /// </summary>
        private void UpdatePagesTable()
        {
            
            Delete
                .Column("NodeCountInSitemap")
                .FromTable("Pages").InSchema(SchemaName);
        }
    }
}