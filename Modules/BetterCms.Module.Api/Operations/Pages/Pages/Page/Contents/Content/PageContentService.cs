using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    public class PageContentService : IPageContentService
    {
        private IPageContentOptionsService optionsService;

        public PageContentService(IPageContentOptionsService optionsService)
        {
            this.optionsService = optionsService;
        }

        IPageContentOptionsService IPageContentService.Options
        {
            get { return optionsService; }
        }
    }
}