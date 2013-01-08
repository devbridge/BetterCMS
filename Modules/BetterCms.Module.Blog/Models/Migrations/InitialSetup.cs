using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(1)]
    public class InitialSetup : DefaultMigration
    {
        private const string PagesModuleSchemaName = "bcms_pages";

        public InitialSetup()
            : base(BlogModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateBlogPostsTable();
        }

        public override void Down()
        {
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
                .ToTable("Pages").InSchema(PagesModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveBlogPostsTable()
        {
            Delete.ForeignKey("FK_Cms_BlogPosts_Cms_PagesPages").OnTable("BlogPosts").InSchema(SchemaName);
            Delete.Table("BlogPosts").InSchema(SchemaName);
        }
    }
}