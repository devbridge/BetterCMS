using BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.History;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content
{
    public class ContentService : Service, IContentService
    {
        private readonly IHtmlContentService htmlService;

        private readonly IBlogPostContentService blogPostService;

        private readonly IContentHistoryService historyService;

        public ContentService(IHtmlContentService htmlService, IBlogPostContentService blogPostService, IContentHistoryService historyService)
        {
            this.htmlService = htmlService;
            this.blogPostService = blogPostService;
            this.historyService = historyService;
        }

        IHtmlContentService IContentService.Html
        {
            get
            {
                return htmlService;
            }
        }

        IBlogPostContentService IContentService.BlogPost
        {
            get
            {
                return blogPostService;
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