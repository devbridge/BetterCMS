using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Module.Templates;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// This migration script creates default / 404 / 500 pages.
    /// </summary>
    [Migration(201301020833)]
    public class Migration201301020833 : DefaultMigration
    {
        /// <summary>
        /// The root schema name.
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201301020833" /> class.
        /// </summary>
        public Migration201301020833()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Creates pages.
        /// </summary>
        public override void Up()
        {
            InsertDefaultPage();

            InsertPage404();

            InsertPage500();
        }

        /// <summary>
        /// Drops pages.
        /// </summary>
        public override void Down()
        {
            RemoveDefaultPage();
            RemovePage404();
            RemovePage500();
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
                .IntoTable("Pages").InSchema(rootSchemaName)
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
                        ShowTitle = true,
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
                .IntoTable("Contents").InSchema(rootSchemaName)
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
                .IntoTable("PageContents").InSchema(rootSchemaName)
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
                .InSchema(rootSchemaName)
                .Row(new { PageId = pageId });

            Delete
                .FromTable("Pages")
                .InSchema(SchemaName)
                .Row(new { Id = pageId });

            Delete
                .FromTable("Pages")
                .InSchema(rootSchemaName)
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
                .InSchema(rootSchemaName)
                .Row(new { Id = contentId });

            Delete
                .FromTable("HtmlContents")
                .InSchema(SchemaName)
                .Row(new { Id = contentId });

            Delete
                .FromTable("Contents")
                .InSchema(rootSchemaName)
                .Row(new { Id = contentId });
        }
    }
}