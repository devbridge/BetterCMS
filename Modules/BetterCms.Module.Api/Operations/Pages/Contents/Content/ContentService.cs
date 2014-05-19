using BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.History;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content
{
    public class ContentService : Service, IContentService
    {
        private readonly IHtmlContentService htmlService;

        private readonly IContentHistoryService historyService;
        
        private readonly IContentDraftService draftService;

        public ContentService(IHtmlContentService htmlService, IContentHistoryService historyService, IContentDraftService draftService)
        {
            this.htmlService = htmlService;
            this.historyService = historyService;
            this.draftService = draftService;
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
        
        IContentDraftService IContentService.Draft
        {
            get
            {
                return draftService;
            }
        }
    }
}