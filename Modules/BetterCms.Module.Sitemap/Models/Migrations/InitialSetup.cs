using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Sitemap.Models.Migrations
{
    /// <summary>
    /// Database structure setup.
    /// </summary>
    [Migration(1)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup() : base(SitemapModuleDescriptor.ModuleName)
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
        /// The down.
        /// </summary>
        public override void Down()
        {
            RemoveSitemapNodesTable();
        }

        /// <summary>
        /// The create sitemap nodes table.
        /// </summary>
        private void CreateSitemapNodesTable()
        {
            Create
               .Table("SitemapNodes")
               .InSchema(SchemaName)

               .WithCmsBaseColumns();

               // TODO:
               // .WithColumn("FirstName").AsString(MaxLength.Name).NotNullable()
        }

        /// <summary>
        /// The remove sitemap nodes table.
        /// </summary>
        private void RemoveSitemapNodesTable()
        {
            Delete.Table("SitemapNodes").InSchema(SchemaName);
        }
    }
}