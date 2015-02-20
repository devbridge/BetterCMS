using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Database structure setup.
    /// </summary>
    [Migration(201303041556)]
    public class Migration201303041556: DefaultMigration
    {        
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
            CreateSitemapNodesTable();            
        }

        /// <summary>
        /// The create sitemap nodes table.
        /// </summary>
        private void CreateSitemapNodesTable()
        {
            Create
               .Table("SitemapNodes")
               .InSchema(SchemaName)

               .WithBaseColumns()
               .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
               .WithColumn("Url").AsString(MaxLength.Url).NotNullable()
               .WithColumn("DisplayOrder").AsInt32().NotNullable()
               .WithColumn("ParentNodeId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_SitemapNodes_ParentNodeId_SitemapNodes_Id")
                .FromTable("SitemapNodes").InSchema(SchemaName).ForeignColumn("ParentNodeId")
                .ToTable("SitemapNodes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_SitemapNodes_Title")
                .OnTable("SitemapNodes").InSchema(SchemaName).OnColumn("Title").Ascending();

            Create
                .Index("IX_Cms_SitemapNodes_ParentNodeId")
                .OnTable("SitemapNodes").InSchema(SchemaName).OnColumn("ParentNodeId").Ascending();
        }
    }
}