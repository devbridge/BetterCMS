using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
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

        /// <summary>
        /// The media manager schema name.
        /// </summary>
        private readonly string mediaManagerSchemaName;

        public InitialSetup()
            : base(BlogModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new RootVersionTableMetaData()).SchemaName;
            pagesModuleSchemaName = (new PagesVersionTableMetaData()).SchemaName;
            mediaManagerSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            CreateAuthorsTable();
            CreateBlogPostsTable();
            CreateOptionsTable();
            CreateBlogPostContentsTable();
        }

        public override void Down()
        {
            RemoveBlogPostContentsTable();
            RemoveOptionsTable();
            RemoveBlogPostsTable();
            RemoveAuthorsTable();
        }

        private void CreateAuthorsTable()
        {
            Create
               .Table("Authors")
               .InSchema(SchemaName)

               .WithCmsBaseColumns()

               .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
               .WithColumn("ImageId").AsGuid().Nullable();

            Create.ForeignKey("FK_Cms_Authors_ImageId_MediaImages_Id")
               .FromTable("Authors").InSchema(SchemaName).ForeignColumn("ImageId")
               .ToTable("MediaImages").InSchema(mediaManagerSchemaName).PrimaryColumn("Id");
        }

        private void RemoveAuthorsTable()
        {
            Delete.ForeignKey("FK_Cms_Authors_ImageId_MediaImages_Id").OnTable("Authors").InSchema(SchemaName);
            Delete.Table("Authors").InSchema(SchemaName);
        }

        private void CreateBlogPostsTable()
        {
            Create
               .Table("BlogPosts")
               .InSchema(SchemaName)
               .WithColumn("Id").AsGuid().PrimaryKey()
               .WithColumn("AuthorId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_BlogPosts_Cms_PagesPages")
                .FromTable("BlogPosts").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Pages").InSchema(pagesModuleSchemaName).PrimaryColumn("Id");

            Create
               .ForeignKey("FK_Cms_BlogPosts_Cms_Authors")
               .FromTable("BlogPosts").InSchema(SchemaName).ForeignColumn("AuthorId")
               .ToTable("Authors").InSchema(SchemaName).PrimaryColumn("Id");
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

        private void CreateBlogPostContentsTable()
        {
            Create
                .Table("BlogPostContents").InSchema(SchemaName)
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey();

            Create
                 .ForeignKey("FK_Cms_BlogPostContents_Cms_HtmlContents")
                 .FromTable("BlogPostContents").InSchema(SchemaName).ForeignColumn("Id")
                 .ToTable("HtmlContents").InSchema(pagesModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveBlogPostContentsTable()
        {
            Delete
                .ForeignKey("FK_Cms_BlogPostContents_Cms_HtmlContents")
                .OnTable("BlogPostContents").InSchema(SchemaName);

            Delete
                .Table("BlogPostContents").InSchema(SchemaName);
        }
    }
}