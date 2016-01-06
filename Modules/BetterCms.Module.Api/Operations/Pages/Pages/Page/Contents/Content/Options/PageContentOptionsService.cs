using BetterModules.Core.DataAccess;
using BetterCms.Module.Root.Services;
using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options
{
    public class PageContentOptionsService : Service, IPageContentOptionsService
    {
        private readonly IRepository repository;
        
        private readonly IOptionService optionService;

        public PageContentOptionsService(IRepository repository, IOptionService optionService)
        {
            this.repository = repository;
            this.optionService = optionService;
        }

        public GetPageContentOptionsResponse Get(GetPageContentOptionsRequest request)
        {
            var results = PageContentOptionsHelper.GetPageContentOptionsResponse(repository, request.PageContentId, request, optionService);
            
            return new GetPageContentOptionsResponse { Data = results };
        }
    }
}