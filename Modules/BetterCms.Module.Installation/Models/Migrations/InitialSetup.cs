using System;

using BetterCms.Configuration;
using BetterCms.Core;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Models.Migrations;
using BetterCms.Module.Root.Models.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Installation.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201305221458)]
    public class InitialSetup : DefaultMigration
    {
        private const string LayoutsTableName = "Layouts";

        private readonly string rootSchemaName;

        private readonly string pagesSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup() : base(InstallationModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new RootVersionTableMetaData()).SchemaName;
            pagesSchemaName = (new PagesVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            if (Schema.Schema("dbo").Exists() && Schema.Schema("dbo").Table("VersionInfo").Exists())
            {
                // Initial setup for the Installation module already executed.
                return;                
            }

            CreateTables();
        }

        private void CreateTables()
        {
            ChangeTemplatesPaths();

            #region Default Layout, Regions and Widgets

            var defaultLayout = new
            {
                Layout = new
                {

                    Id = "0E991684-003A-43DE-B40F-0FFECCDDC6EB",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = "Default Better CMS Template",
                    LayoutPath = "~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml"
                },

                Regions = new
                {
                    Header = new
                    {
                        Id = "9E1601E4-EFCD-4EBB-AE67-B2FF10E372BA",
                        Version = 1,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedByUser = "Better CMS",
                        ModifiedOn = DateTime.Now,
                        ModifiedByUser = "Better CMS",
                        RegionIdentifier = "CMSHeader"
                    },

                    Main = new
                    {
                        Id = "E3E2E7FE-62DF-4BA6-8321-6FDCC1691D8A",
                        Version = 1,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedByUser = "Better CMS",
                        ModifiedOn = DateTime.Now,
                        ModifiedByUser = "Better CMS",
                        RegionIdentifier = "CMSMainContent"
                    },

                    Footer = new
                    {
                        Id = "D840205E-9580-4906-B6B7-B9A48CBF8AAA",
                        Version = 1,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedByUser = "Better CMS",
                        ModifiedOn = DateTime.Now,
                        ModifiedByUser = "Better CMS",
                        RegionIdentifier = "CMSFooter"
                    }
                }
            };

            var defaultLayoutRegions = new
            {
                Header = new
                {
                    Id = "3DA91AD5-D1EC-43D1-B663-7B367C04CDFE",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    LayoutId = defaultLayout.Layout.Id,
                    RegionId = defaultLayout.Regions.Header.Id
                },

                Main = new
                {
                    Id = "40F27280-057E-48DB-8787-3FACF67B98B0",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    LayoutId = defaultLayout.Layout.Id,
                    RegionId = defaultLayout.Regions.Main.Id
                },

                Footer = new
                {
                    Id = "2E7BEBD1-A92E-4D26-B728-239F007F5685",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    LayoutId = defaultLayout.Layout.Id,
                    RegionId = defaultLayout.Regions.Footer.Id
                }
            };

            var widgets = new
            {
                Logo = new
                {
                    ForRootSchemaContentTable = new
                    {
                        Id = "AFA0AFEF-6D71-4962-9EF4-324BB9344F92",
                        Version = 1,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedByUser = "Better CMS",
                        ModifiedOn = DateTime.Now,
                        ModifiedByUser = "Better CMS",
                        Name = "Header Logo",
                        Status = 3,
                        PublishedOn = DateTime.Now,
                        PublishedByUser = "Better CMS"
                    },

                    ForRootScemaWidgetsTable = new
                    {
                        Id = "AFA0AFEF-6D71-4962-9EF4-324BB9344F92",
                    },

                    ForPagesSchemaHtmlContentWidgetsTable = new
                    {
                        Id = "AFA0AFEF-6D71-4962-9EF4-324BB9344F92",
                        UseCustomCss = false,
                        UseCustomJs = false,
                        Html =
                            "<a href=\"/\" class=\"bcms-logo\"><img src=\"/file/bcms-pages/content/styles/images/logo.png\" alt=\"Better CMS\"></a>",
                        UseHtml = true,
                        EditInSourceMode = false
                    }
                },

                Copyright = new
                {
                    ForRootSchemaContentTable = new
                    {
                        Id = "F4A92939-61C1-4E83-9576-42753B8FC15E",
                        Version = 1,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedByUser = "Better CMS",
                        ModifiedOn = DateTime.Now,
                        ModifiedByUser = "Better CMS",
                        Name = "Footer Copyright",
                        Status = 3,
                        PublishedOn = DateTime.Now,
                        PublishedByUser = "Better CMS"
                    },

                    ForRootScemaWidgetsTable = new
                    {
                        Id = "F4A92939-61C1-4E83-9576-42753B8FC15E",
                    },

                    ForPagesSchemaHtmlContentWidgetsTable = new
                    {
                        Id = "F4A92939-61C1-4E83-9576-42753B8FC15E",
                        UseCustomCss = false,
                        UseCustomJs = false,
                        Html =
                            string.Format(
                                "<span class=\"copyright\">Better CMS {0} ©</span>",
                                DateTime.Now.Year),
                        UseHtml = true,
                        EditInSourceMode = false
                    }
                }
            };

            #endregion

            #region Page 404

            var page404 = new
            {
                ForRootSchema = new
                {
                    Id = "468CB682-E1C4-4876-84A5-6A355A92DC55",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PublishedOn = DateTime.Now,
                    LayoutId = defaultLayout.Layout.Id,
                    PageUrl = FixUrl("/404/"),
                    Title = "Page Not Found",
                    Status = (int)PageStatus.Published,
                    MetaTitle = "Page Not Found",
                    MetaDescription = "A page was not found.",
                    MetaKeywords = "Better CMS"
                },

                ForPagesSchema = new
                {
                    Id = "468CB682-E1C4-4876-84A5-6A355A92DC55",
                    Description = "A page was not found.",
                    UseCanonicalUrl = false,
                    UseNoFollow = true,
                    UseNoIndex = true,
                    NodeCountInSitemap = 0
                },

                Content = new
                {
                    ForRootSchemaContentTable = new
                    {
                        Id = "A9C86AB6-DF05-459C-8B28-86814CAD1D47",
                        Version = 1,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedByUser = "Better CMS",
                        ModifiedOn = DateTime.Now,
                        ModifiedByUser = "Better CMS",
                        Name = "Page not found message",
                        Status = 3,
                        PublishedOn = DateTime.Now,
                        PublishedByUser = "Better CMS"
                    },

                    ForPagesSchemaHtmlContentTable = new
                    {
                        Id = "A9C86AB6-DF05-459C-8B28-86814CAD1D47",
                        ActivationDate = DateTime.Now,
                        UseCustomCss = false,
                        UseCustomJs = false,
                        Html = "<p>Oops! The page you are looking for can not be found.</p>",
                        EditInSourceMode = false
                    }
                }
            };

            var page404PageContents = new
            {
                Header = new
                {
                    Id = "1471FC94-D8CC-4FDB-B5D2-0806A91F0B7E",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PageId = page404.ForRootSchema.Id,
                    ContentId = widgets.Logo.ForRootSchemaContentTable.Id,
                    RegionId = defaultLayout.Regions.Header.Id
                },

                Main = new
                {
                    Id = "1786715D-B46E-4F57-A67F-9A77BB0E835B",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PageId = page404.ForRootSchema.Id,
                    ContentId = page404.Content.ForRootSchemaContentTable.Id,
                    RegionId = defaultLayout.Regions.Main.Id
                },

                Footer = new
                {
                    Id = "51AC147A-C915-46D3-ABC9-08D9D215A1B0",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PageId = page404.ForRootSchema.Id,
                    ContentId = widgets.Copyright.ForRootSchemaContentTable.Id,
                    RegionId = defaultLayout.Regions.Footer.Id
                }
            };
            #endregion

            #region Page 500

            var page500 = new
            {
                ForRootSchema = new
                {
                    Id = "7329B110-4E6F-4A1E-B89D-0CB0C1299B73",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PublishedOn = DateTime.Now,
                    LayoutId = defaultLayout.Layout.Id,
                    PageUrl = FixUrl("/500/"),
                    Title = "Internal Server Error",
                    Status = (int)PageStatus.Published,
                    MetaTitle = "Internal Server Error",
                    MetaDescription = "Internal Server Error.",
                    MetaKeywords = "Better CMS"
                },

                ForPagesSchema = new
                {
                    Id = "7329B110-4E6F-4A1E-B89D-0CB0C1299B73",
                    Description = "Internal Server Error.",
                    UseCanonicalUrl = false,
                    UseNoFollow = true,
                    UseNoIndex = true,
                    NodeCountInSitemap = 0
                },

                Content = new
                {
                    ForRootSchemaContentTable = new
                    {
                        Id = "0C8FB3FE-58E9-4D49-975C-2DA934661961",
                        Version = 1,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedByUser = "Better CMS",
                        ModifiedOn = DateTime.Now,
                        ModifiedByUser = "Better CMS",
                        Name = "Internal server error",
                        Status = 3,
                        PublishedOn = DateTime.Now,
                        PublishedByUser = "Better CMS"
                    },

                    ForPagesSchemaHtmlContentTable = new
                    {
                        Id = "0C8FB3FE-58E9-4D49-975C-2DA934661961",
                        ActivationDate = DateTime.Now,
                        UseCustomCss = false,
                        UseCustomJs = false,
                        Html = "<p>Oops! The Web server encountered an unexpected condition that prevented it from fulfilling your request. Please try again later or contact the administrator.</p>",
                        EditInSourceMode = false
                    }
                }
            };

            var page500PageContents = new
            {
                Header = new
                {
                    Id = "6F303E34-2EF8-4DA6-9816-EF46DF6E7A49",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PageId = page500.ForRootSchema.Id,
                    ContentId = widgets.Logo.ForRootSchemaContentTable.Id,
                    RegionId = defaultLayout.Regions.Header.Id
                },

                Main = new
                {
                    Id = "97700E1F-5B7A-4EAC-BE9A-EFA3FAAC6E9F",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PageId = page500.ForRootSchema.Id,
                    ContentId = page500.Content.ForRootSchemaContentTable.Id,
                    RegionId = defaultLayout.Regions.Main.Id
                },

                Footer = new
                {
                    Id = "52381E7F-8AAC-4035-ACA6-76F2BAAD4C74",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PageId = page500.ForRootSchema.Id,
                    ContentId = widgets.Copyright.ForRootSchemaContentTable.Id,
                    RegionId = defaultLayout.Regions.Footer.Id
                }
            };

            #endregion

            #region Default page

            var defaultPage = new
            {
                ForRootSchema = new
                {
                    Id = "7A1867D8-E8D1-4C95-A9F2-FDDF02277C3A",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PublishedOn = DateTime.Now,
                    LayoutId = defaultLayout.Layout.Id,
                    PageUrl = FixUrl("/"),
                    Title = "Default page",
                    Status = (int)PageStatus.Published,
                    MetaTitle = "Better CMS default page",
                    MetaDescription = "Better CMS default page.",
                    MetaKeywords = "Better CMS, Open Source, .net"
                },

                ForPagesSchema = new
                {
                    Id = "7A1867D8-E8D1-4C95-A9F2-FDDF02277C3A",
                    Description = "Default page",
                    UseCanonicalUrl = false,
                    UseNoFollow = false,
                    UseNoIndex = false,
                    NodeCountInSitemap = 0
                },

                Content = new
                {
                    ForRootSchemaContentTable = new
                    {
                        Id = "8243974F-A0E6-4AA6-80E6-AB914821E724",
                        Version = 1,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedByUser = "Better CMS",
                        ModifiedOn = DateTime.Now,
                        ModifiedByUser = "Better CMS",
                        Name = "Content",
                        Status = 3,
                        PublishedOn = DateTime.Now,
                        PublishedByUser = "Better CMS"
                    },

                    ForPagesSchemaHtmlContentTable = new
                    {
                        Id = "8243974F-A0E6-4AA6-80E6-AB914821E724",
                        ActivationDate = DateTime.Now,
                        UseCustomCss = false,
                        UseCustomJs = false,
                        Html = "<p>Hello world!</p>",
                        EditInSourceMode = false
                    }
                }
            };

            var defaultPageContents = new
            {
                Header = new
                {
                    Id = "7337EC22-5465-4792-AD8C-8CA1F18F5842",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PageId = defaultPage.ForRootSchema.Id,
                    ContentId = widgets.Logo.ForRootSchemaContentTable.Id,
                    RegionId = defaultLayout.Regions.Header.Id
                },

                Main = new
                {
                    Id = "12A286B8-3545-4D5D-AD60-EFE2F4475807",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PageId = defaultPage.ForRootSchema.Id,
                    ContentId = defaultPage.Content.ForRootSchemaContentTable.Id,
                    RegionId = defaultLayout.Regions.Main.Id
                },

                Footer = new
                {
                    Id = "07283A5B-8964-400C-9191-89FF982ABEEF",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    PageId = defaultPage.ForRootSchema.Id,
                    ContentId = widgets.Copyright.ForRootSchemaContentTable.Id,
                    RegionId = defaultLayout.Regions.Footer.Id
                }
            };

            #endregion

            var add404 = CmsContext.Config.Installation.Install404ErrorPage;

            var add500 = CmsContext.Config.Installation.Install500ErrorPage;

            var addDefault = CmsContext.Config.Installation.InstallDefaultPage;

            if (!add404 && !add500 && !addDefault)
            {
                return;
            }

            // Add Default Better CMS Layout and regions.
            Insert.IntoTable("Layouts").InSchema(rootSchemaName).Row(defaultLayout.Layout);
            Insert.IntoTable("Regions").InSchema(rootSchemaName)
                .Row(defaultLayout.Regions.Header)
                .Row(defaultLayout.Regions.Main)
                .Row(defaultLayout.Regions.Footer);
            Insert.IntoTable("LayoutRegions").InSchema(rootSchemaName)
                .Row(defaultLayoutRegions.Header)
                .Row(defaultLayoutRegions.Main)
                .Row(defaultLayoutRegions.Footer);

            // Add header logo widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widgets.Logo.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widgets.Logo.ForRootScemaWidgetsTable);
            Insert.IntoTable("HtmlContentWidgets").InSchema(pagesSchemaName).Row(widgets.Logo.ForPagesSchemaHtmlContentWidgetsTable);

            // Add footer copyright widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widgets.Copyright.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widgets.Copyright.ForRootScemaWidgetsTable);
            Insert.IntoTable("HtmlContentWidgets").InSchema(pagesSchemaName).Row(widgets.Copyright.ForPagesSchemaHtmlContentWidgetsTable);

            if (add404)
            {
                // Add 404 page.
                Insert.IntoTable("Pages").InSchema(rootSchemaName).Row(page404.ForRootSchema);
                Insert.IntoTable("Pages").InSchema(pagesSchemaName).Row(page404.ForPagesSchema);

                // Add 404 page contents.
                Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(page404.Content.ForRootSchemaContentTable);
                Insert.IntoTable("HtmlContents").InSchema(pagesSchemaName).Row(page404.Content.ForPagesSchemaHtmlContentTable);

                Insert.IntoTable("PageContents").InSchema(rootSchemaName).Row(page404PageContents.Header);
                Insert.IntoTable("PageContents").InSchema(rootSchemaName).Row(page404PageContents.Main);
                Insert.IntoTable("PageContents").InSchema(rootSchemaName).Row(page404PageContents.Footer);
            }

            if (add500)
            {
                // Add 500 page.
                Insert.IntoTable("Pages").InSchema(rootSchemaName).Row(page500.ForRootSchema);
                Insert.IntoTable("Pages").InSchema(pagesSchemaName).Row(page500.ForPagesSchema);

                // Add 500 page contents.
                Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(page500.Content.ForRootSchemaContentTable);
                Insert.IntoTable("HtmlContents").InSchema(pagesSchemaName).Row(page500.Content.ForPagesSchemaHtmlContentTable);

                Insert.IntoTable("PageContents").InSchema(rootSchemaName).Row(page500PageContents.Header);
                Insert.IntoTable("PageContents").InSchema(rootSchemaName).Row(page500PageContents.Main);
                Insert.IntoTable("PageContents").InSchema(rootSchemaName).Row(page500PageContents.Footer);
            }

            if (addDefault)
            {
                // Add default page.
                Insert.IntoTable("Pages").InSchema(rootSchemaName).Row(defaultPage.ForRootSchema);
                Insert.IntoTable("Pages").InSchema(pagesSchemaName).Row(defaultPage.ForPagesSchema);

                // Add default page contents.
                Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(defaultPage.Content.ForRootSchemaContentTable);
                Insert.IntoTable("HtmlContents").InSchema(pagesSchemaName).Row(defaultPage.Content.ForPagesSchemaHtmlContentTable);

                Insert.IntoTable("PageContents").InSchema(rootSchemaName).Row(defaultPageContents.Header);
                Insert.IntoTable("PageContents").InSchema(rootSchemaName).Row(defaultPageContents.Main);
                Insert.IntoTable("PageContents").InSchema(rootSchemaName).Row(defaultPageContents.Footer);
            }
        }

        private void ChangeTemplatesPaths()
        {
            // For backward compatibility.

            Update.Table(LayoutsTableName)
                  .InSchema(rootSchemaName)
                  .Set(new { LayoutPath = "~/Areas/bcms-installation/Views/Shared/WideLayout.cshtml" })
                  .Where(new { LayoutPath = "~/Areas/bcms-templates/Views/Shared/WideLayout.cshtml" });

            Update.Table(LayoutsTableName)
                  .InSchema(rootSchemaName)
                  .Set(new { LayoutPath = "~/Areas/bcms-installation/Views/Shared/TwoColumnsLayout.cshtml" })
                  .Where(new { LayoutPath = "~/Areas/bcms-templates/Views/Shared/TwoColumnsLayout.cshtml" });

            Update.Table(LayoutsTableName)
                  .InSchema(rootSchemaName)
                  .Set(new { LayoutPath = "~/Areas/bcms-installation/Views/Shared/ThreeColumnsLayout.cshtml" })
                  .Where(new { LayoutPath = "~/Areas/bcms-templates/Views/Shared/ThreeColumnsLayout.cshtml" });
        }

        private static string FixUrl(string url)
        {
            if (url.Trim() == "/")
            {
                return url;
            }

            switch (CmsContext.Config.UrlMode)
            {
                case TrailingSlashBehaviorType.NoTrailingSlash:
                    return url.TrimEnd('/');

                case TrailingSlashBehaviorType.TrailingSlash:
                    return url.EndsWith("/") ? url : string.Concat(url, "/");

                default:
                    return url;
            }
        }
    }
}