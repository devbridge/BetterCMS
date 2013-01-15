using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using BetterCms.Module.Templates;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(201301151849)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// The media manager schema name.
        /// </summary>
        private readonly string mediaManagerSchemaName;

        public InitialSetup()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            mediaManagerSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
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

            InsertDefaultPage();
            InsertPage404();
            InsertPage500();

            CreateHtmlContentHistoryTable();
            CreateServerControlWidgetHistoryTable();
            CreateHtmlContentWidgetHistoryTable();
        }

        public override void Down()
        {
            RemoveHtmlContentWidgetHistoryTable();
            RemoveServerControlWidgetHistoryTable();
            RemoveHtmlContentHistoryTable();

            RemoveDefaultPage();
            RemovePage404();
            RemovePage500();

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
                .ToTable("Contents").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
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
                .ToTable("Widgets").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
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
                .ToTable("Widgets").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
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
                .WithColumn("ImageId").AsGuid().Nullable()
                .WithColumn("CanonicalUrl").AsAnsiString(MaxLength.Url).Nullable()
                .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()                
                .WithColumn("UseCanonicalUrl").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsPublic").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("UseCustomCss").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("UseNoFollow").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("UseNoIndex").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("PublishedOn").AsDateTime().Nullable()
                .WithColumn("AuthorId").AsGuid().Nullable()
                .WithColumn("CategoryId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_PagesPages_Cms_RootPages")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Pages").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PagesPages_Cms_Authors")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("AuthorId")
                .ToTable("Authors").InSchema(SchemaName).PrimaryColumn("Id");
                
            Create.ForeignKey("FK_Cms_Pages_CategoryId_Categories_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create.ForeignKey("FK_Cms_Pages_ImageId_MediaImages_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("ImageId")
                .ToTable("MediaImages").InSchema(mediaManagerSchemaName).PrimaryColumn("Id");
        }

        private void RemovePagesTable()
        {
            Delete.ForeignKey("FK_Cms_Pages_ImageId_MediaImages_Id").OnTable("Pages").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_Pages_CategoryId_Categories_Id").OnTable("Pages").InSchema(SchemaName);
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
                .ToTable("Tags").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
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
                .ToTable("Categories").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemovePageCategoriesTable()
        {
            Delete.ForeignKey("FK_Cms_PageCategories_Cms_Pages").OnTable("PageCategories").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageCategories_Cms_Categories").OnTable("PageCategories").InSchema(SchemaName);
            Delete.Table("PageCategories").InSchema(SchemaName);
        }

        private void CreateHtmlContentHistoryTable()
        {
            Create
               .Table("HtmlContentHistory")
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
                .ForeignKey("FK_Cms_HtmlContentHistory_Id_ContentHistory_Id")
                .FromTable("HtmlContentHistory").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("ContentHistory").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveHtmlContentHistoryTable()
        {
            Delete.ForeignKey("FK_Cms_HtmlContentHistory_Id_ContentHistory_Id").OnTable("HtmlContentHistory").InSchema(SchemaName);
            Delete.Table("HtmlContentHistory").InSchema(SchemaName);
        }

        private void CreateServerControlWidgetHistoryTable()
        {
            Create
                .Table("ServerControlWidgetHistory")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Url").AsAnsiString(MaxLength.Url).NotNullable();

            Create
                .ForeignKey("FK_Cms_ServerControlWidgetHistory_Id_WidgetHistory_Id")
                .FromTable("ServerControlWidgetHistory").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("WidgetHistory").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveServerControlWidgetHistoryTable()
        {
            Delete.ForeignKey("FK_Cms_ServerControlWidgetHistory_Id_WidgetHistory_Id").OnTable("ServerControlWidgetHistory").InSchema(SchemaName);
            Delete.Table("ServerControlWidgetHistory").InSchema(SchemaName);
        }

        private void CreateHtmlContentWidgetHistoryTable()
        {
            Create
                .Table("HtmlContentWidgetHistory")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCustomCss").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CustomJs").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCustomJs").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("Html").AsString(MaxLength.Max).NotNullable()
                .WithColumn("UseHtml").AsBoolean().NotNullable().WithDefaultValue(false);

            Create
                .ForeignKey("FK_Cms_HtmlContentWidgetHistory_Id_WidgetHistory_Id")
                .FromTable("HtmlContentWidgetHistory").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("WidgetHistory").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveHtmlContentWidgetHistoryTable()
        {
            Delete.ForeignKey("FK_Cms_HtmlContentWidgetHistory_Id_WidgetHistory_Id").OnTable("HtmlContentWidgetHistory").InSchema(SchemaName);
            Delete.Table("HtmlContentWidgetHistory").InSchema(SchemaName);
        }    

        /// <summary>
        /// Inserts the default page.
        /// </summary>
        private void InsertDefaultPage()
        {
            InsertPage(
                PagesConstants.PageIds.PageDefault,
                TemplatesModuleConstants.TemplateIds.Wide,
                "/",
                "Better CMS",
                "Better CMS main page.",
                "Better CMS main page meta title",
                "Better CMS main page meta keywords",
                "Better CMS main page meta description");

            // Insert header.
            InsertContent(
                PagesConstants.ContentIds.PageDefaultHeader,
                PagesConstants.PageIds.PageDefault,
                TemplatesModuleConstants.RegionIds.Header,
                "Header",
                "<a href=\"/\" class=\"bcms-logo\"><img src=\"/file/bcms-pages/content/css/images/logo.png\" alt=\"Better CMS\"></a>");

            // Insert body.
            InsertContent(
                PagesConstants.ContentIds.PageDefaultBody,
                PagesConstants.PageIds.PageDefault,
                TemplatesModuleConstants.RegionIds.MainContent,
                "Main Content",
                "<p>Hello world!</p>");

            // Insert footer.
            InsertContent(
                PagesConstants.ContentIds.PageDefaultFooter,
                PagesConstants.PageIds.PageDefault,
                TemplatesModuleConstants.RegionIds.Footer,
                "Footer",
                "<span class=\"copyright\">Better CMS 2012 ©</span>");
        }

        /// <summary>
        /// Inserts the page404.
        /// </summary>
        private void InsertPage404()
        {
            InsertPage(
                PagesConstants.PageIds.Page404,
                TemplatesModuleConstants.TemplateIds.Wide,
                "/404/",
                "Page Not Found",
                "Page Not Found",
                "Page Not Found meta title",
                "Page Not Found meta keywords",
                "Page Not Found meta description");

            // Insert header.
            InsertContent(
                PagesConstants.ContentIds.Page404Header,
                PagesConstants.PageIds.Page404,
                TemplatesModuleConstants.RegionIds.Header,
                "Header",
                "<a href=\"/\" class=\"bcms-logo\"><img src=\"/file/bcms-pages/content/css/images/logo.png\" alt=\"Better CMS\"></a>");

            // Insert body.
            InsertContent(
                PagesConstants.ContentIds.Page404Body,
                PagesConstants.PageIds.Page404,
                TemplatesModuleConstants.RegionIds.MainContent,
                "Main Content",
                "<p>Oops! The page you are looking for can not be found.</p>");

            // Insert footer.
            InsertContent(
                PagesConstants.ContentIds.Page404Footer,
                PagesConstants.PageIds.Page404,
                TemplatesModuleConstants.RegionIds.Footer,
                "Footer",
                "<span class=\"copyright\">Better CMS 2012 ©</span>");
        }

        /// <summary>
        /// Inserts the page500.
        /// </summary>
        private void InsertPage500()
        {
            InsertPage(
                PagesConstants.PageIds.Page500,
                TemplatesModuleConstants.TemplateIds.Wide,
                "/500/",
                "Internal server error",
                "Internal server error.",
                "Better CMS main page meta title",
                "Better CMS main page meta keywords",
                "Better CMS main page meta description");

            // Insert header.
            InsertContent(
                PagesConstants.ContentIds.Page500Header,
                PagesConstants.PageIds.Page500,
                TemplatesModuleConstants.RegionIds.Header,
                "Header",
                "<a href=\"/\" class=\"bcms-logo\"><img src=\"/file/bcms-pages/content/css/images/logo.png\" alt=\"Better CMS\"></a>");

            // Insert body.
            InsertContent(
                PagesConstants.ContentIds.Page500Body,
                PagesConstants.PageIds.Page500,
                TemplatesModuleConstants.RegionIds.MainContent,
                "Main Content",
                "<p>Oops! The Web server encountered an unexpected condition that prevented it from fulfilling your request. Please try again later or contact the administrator.</p>");

            // Insert footer.
            InsertContent(
                PagesConstants.ContentIds.Page500Footer,
                PagesConstants.PageIds.Page500,
                TemplatesModuleConstants.RegionIds.Footer,
                "Footer",
                "<span class=\"copyright\">Better CMS 2012 ©</span>");
        }

        /// <summary>
        /// Removes the default page.
        /// </summary>
        private void RemoveDefaultPage()
        {
            RemovePageContent(PagesConstants.ContentIds.PageDefaultHeader);
            RemovePageContent(PagesConstants.ContentIds.PageDefaultBody);
            RemovePageContent(PagesConstants.ContentIds.PageDefaultFooter);
            RemovePage(PagesConstants.PageIds.PageDefault);
        }

        /// <summary>
        /// Removes 404 page.
        /// </summary>
        private void RemovePage404()
        {
            RemovePageContent(PagesConstants.ContentIds.Page404Header);
            RemovePageContent(PagesConstants.ContentIds.Page404Body);
            RemovePageContent(PagesConstants.ContentIds.Page404Footer);
            RemovePage(PagesConstants.PageIds.Page404);
        }

        /// <summary>
        /// Removes 500 page.
        /// </summary>
        private void RemovePage500()
        {
            RemovePageContent(PagesConstants.ContentIds.Page500Header);
            RemovePageContent(PagesConstants.ContentIds.Page500Body);
            RemovePageContent(PagesConstants.ContentIds.Page500Footer);
            RemovePage(PagesConstants.PageIds.Page500);
        }


        /// <summary>
        /// Inserts the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="layoutId">The layout id.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="metaTitle">The meta title.</param>
        /// <param name="metaKeywords">The meta keywords.</param>
        /// <param name="metaDescription">The meta description.</param>
        private void InsertPage(Guid pageId, Guid layoutId, string pageUrl, string title, string description, string metaTitle, string metaKeywords, string metaDescription)
        {
            // Insert page.
            Insert
                .IntoTable("Pages").InSchema(rootModuleSchemaName)
                .Row(new
                {
                    Id = pageId,
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Admin",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Admin",
                    PageUrl = pageUrl,
                    Title = title,
                    LayoutId = layoutId,
                    PublishedOn = DateTime.Now,
                    IsPublished = true,
                    MetaTitle = metaTitle,
                    MetaKeywords = metaKeywords,
                    MetaDescription = metaDescription,
                });

            Insert
                .IntoTable("Pages").InSchema(SchemaName)
                .Row(new
                {
                    Id = pageId,
                    Description = description,                    
                    UseCanonicalUrl = false,
                    IsPublic = true,
                    UseCustomCss = false,
                    UseNoFollow = false,
                    UseNoIndex = false,
                    PublishedOn = DateTime.Now,
                });
        }

        /// <summary>
        /// Inserts the page header.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="regionId">The region id.</param>
        /// <param name="name">The name.</param>
        /// <param name="html">The HTML.</param>
        private void InsertContent(Guid contentId, Guid pageId, Guid regionId, string name, string html)
        {
            Insert
                .IntoTable("Contents").InSchema(rootModuleSchemaName)
                .Row(new
                {
                    Id = contentId,
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Admin",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Admin",
                    Name = name,
                });

            Insert
                .IntoTable("HtmlContents").InSchema(SchemaName)
                .Row(new
                {
                    Id = contentId,
                    ActivationDate = DateTime.Now,
                    UseCustomCss = false,
                    UseCustomJs = false,
                    Html = html,
                });

            Insert
                .IntoTable("PageContents").InSchema(rootModuleSchemaName)
                .Row(new
                {
                    Id = contentId,
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Admin",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Admin",
                    PageId = pageId,
                    ContentId = contentId,
                    RegionId = regionId,
                    Order = 0,
                });
        }

        /// <summary>
        /// Removes the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        private void RemovePage(Guid pageId)
        {
            Delete
                .FromTable("PageContents")
                .InSchema(rootModuleSchemaName)
                .Row(new { PageId = pageId });

            Delete
                .FromTable("Pages")
                .InSchema(SchemaName)
                .Row(new { Id = pageId });

            Delete
                .FromTable("Pages")
                .InSchema(rootModuleSchemaName)
                .Row(new { Id = pageId });
        }

        /// <summary>
        /// Removes the content.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        private void RemovePageContent(Guid contentId)
        {
            Delete
                .FromTable("PageContents")
                .InSchema(rootModuleSchemaName)
                .Row(new { Id = contentId });

            Delete
                .FromTable("HtmlContents")
                .InSchema(SchemaName)
                .Row(new { Id = contentId });

            Delete
                .FromTable("Contents")
                .InSchema(rootModuleSchemaName)
                .Row(new { Id = contentId });
        }
    }
}