using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    public interface IPageService
    {
        IPagePropertiesService Properties { get; }

        IPageContentsService Contents { get; }
        
        IPageTranslationsService Translations { get; }

        IPageContentService Content { get; }

        GetPageResponse Get(GetPageRequest request);

        PageExistsResponse Exists(PageExistsRequest request);
    }
}