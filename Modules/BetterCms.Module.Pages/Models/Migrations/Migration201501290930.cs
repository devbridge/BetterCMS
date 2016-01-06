using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(201501290930)]
    public class Migration201501290930 : DefaultMigration
    {
        private readonly string rootModuleSchemaName;
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201501290930"/> class.
        /// </summary>
        public Migration201501290930()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            CreatePageAccessRulesTable();
        }
        private void CreatePageAccessRulesTable()
        {
            Create
                .Table("PageCategories")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable();           

            Create
                .ForeignKey("FK_Cms_PageCategories_PageId_Cms_Page_Id")
                .FromTable("PageCategories").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageCategories_CategoryId_Cms_Category_Id")
                .FromTable("PageCategories").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_PageCategories_PageId")
                .OnTable("PageCategories").InSchema(SchemaName).OnColumn("PageId");

            Create
                .Index("IX_Cms_PageCategories_AccessRuleId")
                .OnTable("PageCategories").InSchema(SchemaName).OnColumn("CategoryId");
        }
    }
}