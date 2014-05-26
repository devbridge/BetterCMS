using BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.History;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content
{
    public interface IContentService
    {
        IHtmlContentService Html { get; }
        
        IContentHistoryService History { get; }

        IContentDraftService Draft { get; }
    }
}