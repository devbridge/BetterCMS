using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options
{
    public class PageContentOptionsController : ApiController, IPageContentOptionsService
    {
        private readonly IRepository repository;
        
        private readonly IOptionService optionService;

        public PageContentOptionsController(IRepository repository, IOptionService optionService)
        {
            this.repository = repository;
            this.optionService = optionService;
        }
        [Route("bcms-api/pages/contents/{PageContentId}/options")]
        public GetPageContentOptionsResponse Get([ModelBinder(typeof(JsonModelBinder))] GetPageContentOptionsRequest request)
        {
            var results = PageContentOptionsHelper.GetPageContentOptionsResponse(repository, request.PageContentId, request, optionService);
            
            return new GetPageContentOptionsResponse { Data = results };
        }
    }
}