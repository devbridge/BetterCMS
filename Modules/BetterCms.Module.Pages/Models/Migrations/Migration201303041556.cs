using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Database structure setup.
    /// </summary>
    [Migration(201303041556)]
    public class Migration201303041556 : DefaultMigration
    {
        /// <summary>
        /// The navigation module schema name.
        /// </summary>
        private readonly string navigationModuleSchemaName = "bcms_navigation";

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201303041556"/> class.
        /// </summary>
        public Migration201303041556() : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// The up.
        /// </summary>
        public override void Up()
        {
            if (Schema.Schema(navigationModuleSchemaName).Table("SitemapNodes").Exists())
            {
                Alter.Table("SitemapNodes").InSchema(navigationModuleSchemaName).ToSchema(SchemaName);
            }
            else
            {
                CreateSitemapNodesTable(SchemaName);
            }
        }

        /// <summary>
        /// The down.
        /// </summary>
        public override void Down()
        {
            if (Schema.Schema(navigationModuleSchemaName).Table("SitemapNodes").Exists())
            {
                Alter.Table("SitemapNodes").InSchema(SchemaName).ToSchema(navigationModuleSchemaName);
            }
            else
            {
                CreateSitemapNodesTable(navigationModuleSchemaName);
            }
        }

        /// <summary>
        /// The create sitemap nodes table.
        /// </summary>
        /// <param name="schemaName">Name of the schema.</param>
        private void CreateSitemapNodesTable(string schemaName)
        {
            Create
               .Table("SitemapNodes")
               .InSchema(schemaName)

               .WithCmsBaseColumns()
               .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
               .WithColumn("Url").AsString(MaxLength.Url).NotNullable()
               .WithColumn("DisplayOrder").AsInt32().NotNullable()
               .WithColumn("ParentNodeId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_SitemapNodes_ParentNodeId_SitemapNodes_Id")
                .FromTable("SitemapNodes").InSchema(schemaName).ForeignColumn("ParentNodeId")
                .ToTable("SitemapNodes").InSchema(schemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_SitemapNodes_Title")
                .OnTable("SitemapNodes").InSchema(schemaName).OnColumn("Title").Ascending();

            Create
                .Index("IX_Cms_SitemapNodes_ParentNodeId")
                .OnTable("SitemapNodes").InSchema(schemaName).OnColumn("ParentNodeId").Ascending();
        }
    }
}