using BetterCms.Module.Api.Operations.Pages.Contents.Content.History;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content
{
    public class ContentService : Service, IContentService
    {
        private readonly IHtmlContentService htmlService;

        private readonly IContentHistoryService historyService;

        public ContentService(IHtmlContentService htmlService, IContentHistoryService historyService)
        {
            this.htmlService = htmlService;
            this.historyService = historyService;
        }

        IHtmlContentService IContentService.Html
        {
            get
            {
                return htmlService;
            }
        }

        IContentHistoryService IContentService.History
        {
            get
            {
                return historyService;
            }
        }
    }
}