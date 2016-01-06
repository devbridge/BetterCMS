using BetterCms.Module.Pages.Models.Migrations;
using BetterCms.Module.Root.Models.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creations script.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(BlogModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new RootVersionTableMetaData()).SchemaName;
            pagesModuleSchemaName = (new PagesVersionTableMetaData()).SchemaName;
            mediaManagerSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateAuthorsTable();
            CreateBlogPostsTable();
            CreateOptionsTable();
            CreateBlogPostContentsTable();
        }

        /// <summary>
        /// Creates the authors table.
        /// </summary>
        private void CreateAuthorsTable()
        {
            Create
               .Table("Authors")
               .InSchema(SchemaName)

               .WithBaseColumns()

               .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
               .WithColumn("ImageId").AsGuid().Nullable();

            Create.ForeignKey("FK_Cms_Authors_ImageId_MediaImages_Id")
               .FromTable("Authors").InSchema(SchemaName).ForeignColumn("ImageId")
               .ToTable("MediaImages").InSchema(mediaManagerSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the blog posts table.
        /// </summary>
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

        /// <summary>
        /// Creates the options table.
        /// </summary>
        private void CreateOptionsTable()
        {
            Create
               .Table("Options")
               .InSchema(SchemaName)
               .WithBaseColumns()
               .WithColumn("DefaultLayoutId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_BlogOptions_Cms_Layouts")
                .FromTable("Options").InSchema(SchemaName).ForeignColumn("DefaultLayoutId")
                .ToTable("Layouts").InSchema(rootSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the blog post contents table.
        /// </summary>
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
    }
}