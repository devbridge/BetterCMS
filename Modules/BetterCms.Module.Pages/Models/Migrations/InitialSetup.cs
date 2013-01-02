using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(1)]
    public class InitialSetup : DefaultMigration
    {
        private const string RootModuleSchemaName = "bcms_root";

        public InitialSetup()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateAuthorsTable();
            CreateRedirectsTable();
                
            CreateHtmlContentsTable();
            CreateServerControlWidgetsTable();
            CreateHtmlContentWidgetsTable();

            CreatePagesTable();

            CreatePageTagsTable();
            CreatePageCategoriesTable();
        }

        public override void Down()
        {
            RemovePageTagsTable();
            RemovePageCategoriesTable();
            
            RemoveHtmlContentWidgetsTable();
            RemoveServerControlWidgetsTable();
            RemoveHtmlContentsTable();

            RemovePagesTable();

            RemoveRedirectsTable();
            RemoveAuthorsTable();
        }

        private void CreateAuthorsTable()
        {            
            Create
               .Table("Authors")
               .InSchema(SchemaName)

               .WithCmsBaseColumns()

               .WithColumn("FirstName").AsString(MaxLength.Name).NotNullable()
               .WithColumn("LastName").AsString(MaxLength.Name).NotNullable()
               .WithColumn("DisplayName").AsString(MaxLength.Name).NotNullable()

               .WithColumn("Title").AsString(MaxLength.Name).Nullable()
               .WithColumn("Email").AsString(MaxLength.Email).Nullable()
               .WithColumn("Twitter").AsString(MaxLength.Name).Nullable()
               .WithColumn("ProfileImageUrl").AsString(MaxLength.Url).Nullable()
               .WithColumn("ProfileThumbnailUrl").AsString(MaxLength.Url).Nullable()
               .WithColumn("ShortDescription").AsString(MaxLength.Text).Nullable()
               .WithColumn("LongDescription").AsString(MaxLength.Max).Nullable();               
        }

        private void RemoveAuthorsTable()
        {
            Delete.Table("Authors").InSchema(SchemaName);
        }

        private void CreateRedirectsTable()
        {
            Create
                .Table("Redirects")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("PageUrl").AsString(MaxLength.Url).NotNullable()
                .WithColumn("RedirectUrl").AsString(MaxLength.Url).NotNullable();                
        }

        private void RemoveRedirectsTable()
        {
            Delete.Table("Redirects").InSchema(SchemaName);
        }

        private void CreateHtmlContentsTable()
        {
            Create
               .Table("HtmlContents")
               .InSchema(SchemaName)
               .WithColumn("Id").AsGuid().PrimaryKey()
               .WithColumn("ActivationDate").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
               .WithColumn("ExpirationDate").AsDateTime().Nullable()
               .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()
               .WithColumn("UseCustomCss").AsBoolean().NotNullable().WithDefaultValue(false)
               .WithColumn("CustomJs").AsString(MaxLength.Max).Nullable()
               .WithColumn("UseCustomJs").AsBoolean().NotNullable().WithDefaultValue(false)
               .WithColumn("Html").AsString(int.MaxValue).NotNullable();               
               
            Create
                .ForeignKey("FK_Cms_HtmlContents_Id_Contents_Id")
                .FromTable("HtmlContents").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Contents").InSchema(RootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveHtmlContentsTable()
        {
            Delete.ForeignKey("FK_Cms_HtmlContents_Id_Contents_Id").OnTable("HtmlContents").InSchema(SchemaName);
            Delete.Table("HtmlContents").InSchema(SchemaName);
        }

        private void CreateServerControlWidgetsTable()
        {
            Create
                .Table("ServerControlWidgets")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()                
                .WithColumn("Url").AsAnsiString(MaxLength.Url).NotNullable();

            Create
                .ForeignKey("FK_Cms_ServerControlWidgets_Id_Widgets_Id")
                .FromTable("ServerControlWidgets").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Widgets").InSchema(RootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveServerControlWidgetsTable()
        {
            Delete.ForeignKey("FK_Cms_ServerControlWidgets_Id_Widgets_Id").OnTable("ServerControlWidgets").InSchema(SchemaName);
            Delete.Table("ServerControlWidgets").InSchema(SchemaName);
        }

        private void CreateHtmlContentWidgetsTable()
        {
            Create
                .Table("HtmlContentWidgets")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCustomCss").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CustomJs").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCustomJs").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("Html").AsString(MaxLength.Max).NotNullable()
                .WithColumn("UseHtml").AsBoolean().NotNullable().WithDefaultValue(false);

            Create
                .ForeignKey("FK_Cms_HtmlContentWidgets_Id_Widgets_Id")
                .FromTable("HtmlContentWidgets").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Widgets").InSchema(RootModuleSchemaName).PrimaryColumn("Id");
        }
     
        private void RemoveHtmlContentWidgetsTable()
        {
            Delete.ForeignKey("FK_Cms_HtmlContentWidgets_Id_Widgets_Id").OnTable("HtmlContentWidgets").InSchema(SchemaName);
            Delete.Table("HtmlContentWidgets").InSchema(SchemaName);
        }

        private void CreatePagesTable()
        {
            Create
                .Table("Pages")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Description").AsString(MaxLength.Text).Nullable()
                .WithColumn("ImageUrl").AsString(MaxLength.Url).Nullable()
                .WithColumn("CanonicalUrl").AsAnsiString(MaxLength.Url).Nullable()
                .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()

                .WithColumn("ShowTitle").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("UseCanonicalUrl").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsPublic").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("UseCustomCss").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("UseNoFollow").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("UseNoIndex").AsBoolean().NotNullable().WithDefaultValue(false)

                .WithColumn("PublishedOn").AsDateTime().Nullable()

                .WithColumn("AuthorId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_PagesPages_Cms_RootPages")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Pages").InSchema(RootModuleSchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PagesPages_Cms_Authors")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("AuthorId")
                .ToTable("Authors").InSchema(SchemaName).PrimaryColumn("Id");
        }

        private void RemovePagesTable()
        {
            Delete.ForeignKey("FK_Cms_PagesPages_Cms_RootPages").OnTable("Pages").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PagesPages_Cms_Authors").OnTable("Pages").InSchema(SchemaName);
            Delete.Table("Pages").InSchema(SchemaName);
        }

        private void CreatePageTagsTable()
        {
            Create
                .Table("PageTags")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("TagId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_PageTags_Cms_Pages")
                .FromTable("PageTags").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageTags_Cms_Tags")
                .FromTable("PageTags").InSchema(SchemaName).ForeignColumn("TagId")
                .ToTable("Tags").InSchema(RootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemovePageTagsTable()
        {
            Delete.ForeignKey("FK_Cms_PageTags_Cms_Pages").OnTable("PageTags").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageTags_Cms_Tags").OnTable("PageTags").InSchema(SchemaName);
            Delete.Table("PageTags").InSchema(SchemaName);
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
                .ToTable("Categories").InSchema(RootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemovePageCategoriesTable()
        {
            Delete.ForeignKey("FK_Cms_PageCategories_Cms_Pages").OnTable("PageCategories").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageCategories_Cms_Categories").OnTable("PageCategories").InSchema(SchemaName);
            Delete.Table("PageCategories").InSchema(SchemaName);
        }
    }
}