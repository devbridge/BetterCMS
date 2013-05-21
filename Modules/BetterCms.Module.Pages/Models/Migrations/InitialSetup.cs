using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using BetterCms.Module.Templates;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            mediaManagerSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateRedirectsTable();
                
            CreateHtmlContentsTable();
            CreateServerControlWidgetsTable();
            CreateHtmlContentWidgetsTable();

            CreatePagesTable();

            CreatePageTagsTable();
            
            InsertDefaultPage();
            InsertPage404();
            InsertPage500();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the redirects table.
        /// </summary>
        private void CreateRedirectsTable()
        {
            Create
                .Table("Redirects")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("PageUrl").AsString(MaxLength.Url).NotNullable()
                .WithColumn("RedirectUrl").AsString(MaxLength.Url).NotNullable();                
        }

        /// <summary>
        /// Creates the HTML contents table.
        /// </summary>
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

        /// <summary>
        /// Creates the server control widgets table.
        /// </summary>
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

        /// <summary>
        /// Creates the HTML content widgets table.
        /// </summary>
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

        /// <summary>
        /// Creates the pages table.
        /// </summary>
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
                .WithColumn("CustomJS").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCanonicalUrl").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsPublic").AsBoolean().NotNullable().WithDefaultValue(true)                
                .WithColumn("UseNoFollow").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("UseNoIndex").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("PublishedOn").AsDateTime().Nullable()                
                .WithColumn("CategoryId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_PagesPages_Cms_RootPages")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Pages").InSchema(rootModuleSchemaName).PrimaryColumn("Id");            
                
            Create.ForeignKey("FK_Cms_Pages_CategoryId_Categories_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create.ForeignKey("FK_Cms_Pages_ImageId_MediaImages_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("ImageId")
                .ToTable("MediaImages").InSchema(mediaManagerSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the page tags table.
        /// </summary>
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
                "<a href=\"/\" class=\"bcms-logo\"><img src=\"/file/bcms-pages/content/styles/images/logo.png\" alt=\"Better CMS\"></a>");

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
                "<a href=\"/\" class=\"bcms-logo\"><img src=\"/file/bcms-pages/content/styles/images/logo.png\" alt=\"Better CMS\"></a>");

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
                "<a href=\"/\" class=\"bcms-logo\"><img src=\"/file/bcms-pages/content/styles/images/logo.png\" alt=\"Better CMS\"></a>");

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
                    Status = 3,
                    PublishedOn = DateTime.Now,
                    PublishedByUser = "Admin"
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
                    Id = Guid.NewGuid(),
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Admin",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Admin",
                    PageId = pageId,
                    ContentId = contentId,
                    RegionId = regionId,
                    Order = 0                    
                });
        }
    }
}