using System.Linq;
using System.Transactions;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Api.Dto;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Installation.Models.MigrationsContent
{
    [ContentMigration(201305231042)]
    public class Migration201305231042 : ContentDefaultMigration
    {
        private static class Urls
        {
            public const string DefaultPage = "/";
            public const string Page404 = "/404/";
            public const string Page500 = "/500/";
        }
        private static class Template
        {
            public const string Name = "Default Better CMS Template";

            public const string LayoutPath = "~/Areas/bcms-installation/Views/Shared/WideLayout.cshtml";

            public static class Regions
            {
                public const string Main = "CMSMainContent";

                public const string Header = "CMSHeader";

                public const string Footer = "CMSFooter";
            }
        }

        public override void Up(ICmsConfiguration configuration)
        {
            using (var pagesApi = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                var add404 = configuration.Installation.Install404ErrorPage && !pagesApi.GetPages(page => page.PageUrl == Urls.Page404, includeUnpublished: true, includePrivate: true).Any();
                var add500 = configuration.Installation.Install500ErrorPage && !pagesApi.GetPages(page => page.PageUrl == Urls.Page500, includeUnpublished: true, includePrivate: true).Any();
                var addDefault = configuration.Installation.InstallDefaultPage && !pagesApi.GetPages(page => page.PageUrl == Urls.DefaultPage, includeUnpublished: true, includePrivate: true).Any();

                if (!add404 && !add500 && !addDefault)
                {
                    return;
                }

                using (var transactionScope = new TransactionScope())
                {
                    var layout = AddLayout(pagesApi);
                    var headerWidget = AddHtmlWidget(pagesApi, "Header", "<a href=\"/\" class=\"bcms-logo\"><img src=\"/file/bcms-pages/content/styles/images/logo.png\" alt=\"Better CMS\"></a>");
                    var footerWidget = AddHtmlWidget(pagesApi, "Footer", "<span class=\"copyright\">Better CMS 2012 ©</span>");

                    if (add404)
                    {
                        Add404ErrorPage(pagesApi, layout, headerWidget, footerWidget);
                    }

                    if (add500)
                    {
                        Add500ErrorPage(pagesApi, layout, headerWidget, footerWidget);
                    }

                    if (addDefault)
                    {
                        AddDefaultPage(pagesApi, layout, headerWidget, footerWidget);
                    }

                    transactionScope.Complete();
                }
            }
        }

        private static HtmlContentWidget AddHtmlWidget(PagesApiContext pagesApi, string title, string html)
        {
            var name = string.Format("Default Better CMS {0}", title);

            var widgets = pagesApi.GetHtmlContentWidgets(e => e.Name == name);
            if (widgets.Count > 0)
            {
                return widgets[0];
            }

            var request = new CreateHtmlContentWidgetRequest() { Name = name, Html = html };
            return pagesApi.CreateHtmlContentWidget(request);
        }

        private static Layout AddLayout(PagesApiContext pagesApi)
        {
            var layouts = pagesApi.GetLayouts(l => l.LayoutPath == Template.LayoutPath);
            if (layouts.Count > 0)
            {
                return layouts[0];
            }

            var request = new CreateLayoutRequest()
            {
                Name = Template.Name,
                LayoutPath = Template.LayoutPath,
                Regions = new[] { Template.Regions.Main, Template.Regions.Header, Template.Regions.Footer }
            };
            return pagesApi.CreateLayout(request);
        }

        private static void Add404ErrorPage(PagesApiContext pagesApi, Layout layout, HtmlContentWidget header, HtmlContentWidget footer)
        {
            var pageRequest = new CreatePageRequest()
            {
                LayoutId = layout.Id,
                PageUrl = Urls.Page404,
                Title = "Page Not Found",
                Description = "Page Not Found",
                Status = PageStatus.Published,
                MetaTitle = "Better CMS page not found meta title",
                MetaDescription = "Better CMS page not found meta description.",
                MetaKeywords = "Better CMS"
            };
            var page = pagesApi.CreatePage(pageRequest);

            AddPageContent(pagesApi, page, header, footer, "<p>Oops! The page you are looking for can not be found.</p>");
        }

        private static void Add500ErrorPage(PagesApiContext pagesApi, Layout layout, HtmlContentWidget header, HtmlContentWidget footer)
        {
            var pageRequest = new CreatePageRequest()
            {
                LayoutId = layout.Id,
                PageUrl = Urls.Page500,
                Title = "Internal server error",
                Description = "Internal server error",
                Status = PageStatus.Published,
                MetaTitle = "Better CMS internal server error meta title",
                MetaDescription = "Better CMS internal server error meta description.",
                MetaKeywords = "Better CMS"
            };
            var page = pagesApi.CreatePage(pageRequest);

            AddPageContent(pagesApi, page, header, footer, "<p>Oops! The Web server encountered an unexpected condition that prevented it from fulfilling your request. Please try again later or contact the administrator.</p>");
        }

        private static void AddDefaultPage(PagesApiContext pagesApi, Layout layout, HtmlContentWidget header, HtmlContentWidget footer)
        {
            var pageRequest = new CreatePageRequest()
            {
                LayoutId = layout.Id,
                PageUrl = Urls.DefaultPage,
                Title = "Better CMS",
                Description = "Better CMS main page.",
                Status = PageStatus.Published,
                MetaTitle = "Better CMS main page meta title",
                MetaDescription = "Better CMS main page meta description.",
                MetaKeywords = "Better CMS"
            };
            var page = pagesApi.CreatePage(pageRequest);

            AddPageContent(pagesApi, page, header, footer, "<p>Hello world!</p>");
        }

        private static void AddPageContent(PagesApiContext pagesApi, PageProperties page, HtmlContentWidget header, HtmlContentWidget footer, string mainHtml)
        {
            pagesApi.AddHtmlContentWidgetToPage(new AddWidgetToPageRequest() { PageId = page.Id, ContentId = header.Id, RegionIdentifier = Template.Regions.Header });
            pagesApi.AddHtmlContentWidgetToPage(new AddWidgetToPageRequest() { PageId = page.Id, ContentId = footer.Id, RegionIdentifier = Template.Regions.Footer });

            var contentRequest = new CreatePageHtmlContentRequest()
            {
                PageId = page.Id,
                RegionIdentifier = Template.Regions.Main,
                Name = "Main Content",
                Html = mainHtml,
                ContentStatus = ContentStatus.Published
            };
            pagesApi.CreatePageHtmlContent(contentRequest);
        }
    }
}