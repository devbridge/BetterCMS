using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201301221400)]
    public class Migration201301221400 : DefaultMigration
    {
        /// <summary>
        /// The pages module schema name.
        /// </summary>
        private readonly string pagesModuleSchemaName;

        public Migration201301221400()
            : base(BlogModuleDescriptor.ModuleName)
        {
            pagesModuleSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            Create
                .Table("BlogPostContents").InSchema(SchemaName)
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey();

            Create
                 .ForeignKey("FK_Cms_BlogPostContents_Cms_HtmlContents")
                 .FromTable("BlogPostContents").InSchema(SchemaName).ForeignColumn("Id")
                 .ToTable("HtmlContents").InSchema(pagesModuleSchemaName).PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete
                .ForeignKey("FK_Cms_BlogPostContents_Cms_HtmlContents")
                .OnTable("BlogPostContents").InSchema(SchemaName);

            Delete
                .Table("BlogPostContents").InSchema(SchemaName);
        }
    }
}