using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(201301171330)]
    public class Migration201301171330 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        public Migration201301171330()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            RemovePageCategoriesTable();
        }

        public override void Down()
        {
            CreatePageCategoriesTable();
        }
        
        private void CreatePageCategoriesTable()
        {
            Create
                .Table("PageCategories")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_PageCategories_Cms_Pages")
                .FromTable("PageCategories").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageCategories_Cms_Categories")
                .FromTable("PageCategories").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemovePageCategoriesTable()
        {
            Delete.ForeignKey("FK_Cms_PageCategories_Cms_Pages").OnTable("PageCategories").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageCategories_Cms_Categories").OnTable("PageCategories").InSchema(SchemaName);
            Delete.Table("PageCategories").InSchema(SchemaName);
        }
    }
}