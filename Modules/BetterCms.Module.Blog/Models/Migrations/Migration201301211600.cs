using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201301211600)]
    public class Migration201301211600 : DefaultMigration
    {
        /// <summary>
        /// The pages module schema name.
        /// </summary>
        private readonly string pagesModuleSchemaName;

        public Migration201301211600()
            : base(BlogModuleDescriptor.ModuleName)
        {
            pagesModuleSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            Alter
                .Table("Authors")
                .InSchema(pagesModuleSchemaName)
                .ToSchema(SchemaName);

            Delete
                .ForeignKey("FK_Cms_PagesPages_Cms_Authors")
                .OnTable("Pages").InSchema(pagesModuleSchemaName);

            Delete
                .Column("AuthorId")
                .FromTable("Pages")
                .InSchema(pagesModuleSchemaName);

            Alter
                .Table("BlogPosts")
                .InSchema(SchemaName)
                .AddColumn("AuthorId")
                .AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_BlogPosts_Cms_Authors")
                .FromTable("BlogPosts").InSchema(SchemaName).ForeignColumn("AuthorId")
                .ToTable("Authors").InSchema(SchemaName).PrimaryColumn("Id");
        }

        public override void Down()
        {
            Alter
                .Table("Authors")
                .InSchema(SchemaName)
                .ToSchema(pagesModuleSchemaName);

            Delete
                .ForeignKey("FK_Cms_BlogPosts_Cms_Authors")
                .OnTable("BlogPosts").InSchema(SchemaName);

            Delete
                .Column("AuthorId")
                .FromTable("BlogPosts")
                .InSchema(SchemaName);

            Alter
                .Table("Pages")
                .InSchema(pagesModuleSchemaName)
                .AddColumn("AuthorId")
                .AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_PagesPages_Cms_Authors")
                .FromTable("Pages").InSchema(pagesModuleSchemaName).ForeignColumn("AuthorId")
                .ToTable("Authors").InSchema(pagesModuleSchemaName).PrimaryColumn("Id");
        }
    }
}