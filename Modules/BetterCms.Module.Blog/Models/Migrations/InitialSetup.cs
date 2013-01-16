using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Module.Pages.Models.Migrations;
using BetterCms.Module.Root.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201301151922)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// The root schema name.
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// The pages module schema name.
        /// </summary>
        private readonly string pagesModuleSchemaName;

        public InitialSetup()
            : base(BlogModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new RootVersionTableMetaData()).SchemaName;
            pagesModuleSchemaName = (new PagesVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            CreateBlogPostsTable();
            CreateOptionsTable();
        }

        public override void Down()
        {
            RemoveOptionsTable();
            RemoveBlogPostsTable();
        }

        private void CreateBlogPostsTable()
        {
            Create
               .Table("BlogPosts")
               .InSchema(SchemaName)

               .WithColumn("Id").AsGuid().PrimaryKey();

            Create
                .ForeignKey("FK_Cms_BlogPosts_Cms_PagesPages")
                .FromTable("BlogPosts").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Pages").InSchema(pagesModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveBlogPostsTable()
        {
            Delete.ForeignKey("FK_Cms_BlogPosts_Cms_PagesPages").OnTable("BlogPosts").InSchema(SchemaName);
            Delete.Table("BlogPosts").InSchema(SchemaName);
        }

        private void CreateOptionsTable()
        {
            Create
               .Table("Options")
               .InSchema(SchemaName)
               .WithCmsBaseColumns()
               .WithColumn("DefaultLayoutId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_BlogOptions_Cms_Layouts")
                .FromTable("Options").InSchema(SchemaName).ForeignColumn("DefaultLayoutId")
                .ToTable("Layouts").InSchema(rootSchemaName).PrimaryColumn("Id");
        }

        private void RemoveOptionsTable()
        {
            Delete.ForeignKey("FK_Cms_BlogOptions_Cms_Layouts").OnTable("Options").InSchema(SchemaName);
            Delete.Table("Options").InSchema(SchemaName);
        } 
    }
}