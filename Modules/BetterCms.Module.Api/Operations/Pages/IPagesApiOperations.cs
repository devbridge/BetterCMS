using BetterCms.Module.Api.Operations.Pages.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget;
using BetterCms.Module.Pages.Services;

using IPageService = BetterCms.Module.Api.Operations.Pages.Pages.Page.IPageService;
using IRedirectService = BetterCms.Module.Api.Operations.Pages.Redirects.Redirect.IRedirectService;
using IWidgetsService = BetterCms.Module.Api.Operations.Pages.Widgets.IWidgetsService;

namespace BetterCms.Module.Api.Operations.Pages
{
    public interface IPagesApiOperations
    {
        IPagesService Pages { get; }
        
        IPageService Page { get; }
        
        IContentService Content { get; }
        
        IWidgetsService Widgets { get; }
        
        IWidgetService Widget { get; }
        
        IRedirectsService Redirects { get; }
        
        IRedirectService Redirect { get; }
        
        ISitemapService Sitemap { get; }
    }
}
