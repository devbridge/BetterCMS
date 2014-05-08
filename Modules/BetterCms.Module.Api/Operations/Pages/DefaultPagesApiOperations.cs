using BetterCms.Module.Api.Operations.Pages.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Sitemaps;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget;

using IPageService = BetterCms.Module.Api.Operations.Pages.Pages.Page.IPageService;
using IRedirectService = BetterCms.Module.Api.Operations.Pages.Redirects.Redirect.IRedirectService;
using ISitemapService = BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.ISitemapService;
using IWidgetsService = BetterCms.Module.Api.Operations.Pages.Widgets.IWidgetsService;

namespace BetterCms.Module.Api.Operations.Pages
{
    public class DefaultPagesApiOperations : IPagesApiOperations
    {
        public DefaultPagesApiOperations(IPagesService pages, IPageService page, IContentService content, IWidgetService widget, IWidgetsService widgets,
            IRedirectsService redirects, IRedirectService redirect, ISitemapService sitemap, ISitemapsService sitemaps)
        {
            Pages = pages;
            Page = page;
            Content = content;
            Widget = widget;
            Widgets = widgets;
            Redirect = redirect;
            Redirects = redirects;
            Sitemap = sitemap;
            Sitemaps = sitemaps;
        }

        public IPagesService Pages
        {
            get; 
            private set;
        }
        
        public IPageService Page
        {
            get; 
            private set;
        }
        
        public IContentService Content
        {
            get; 
            private set;
        }


        public IWidgetsService Widgets
        {
            get;
            private set;
        }

        public IWidgetService Widget
        {
            get;
            private set;
        }

        public IRedirectsService Redirects
        {
            get;
            private set;
        }

        public IRedirectService Redirect
        {
            get;
            private set;
        }

        public ISitemapsService Sitemaps
        {
            get;
            private set;
        }

        public ISitemapService Sitemap
        {
            get;
            private set;
        }
    }
}
