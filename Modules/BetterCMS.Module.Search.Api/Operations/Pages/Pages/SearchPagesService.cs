using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Pages.Search;

using BetterCms.Module.Search.Models;
using BetterCms.Module.Search.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Search.Api.Operations.Pages.Pages
{
    [RoutePrefix("bcms-api")]
    public class SearchPagesController : ApiController, ISearchPagesService
    {
        private readonly ISearchService searchService;

        public SearchPagesController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [Route("pages/search/{SearchString}")]
        public SearchPagesResponse Get([ModelBinder(typeof(JsonModelBinder))]SearchPagesRequest request)
        {
            var take = request.Data.Take ?? 10;
            var skip = request.Data.Skip > 0 ? request.Data.Skip : 0;

            var results = searchService.Search(new SearchRequest(request.SearchString, take, skip));

            var items =
                results.Items.Select(
                    r => new SearchResultModel
                    {
                        Title = r.Title, 
                        Link = r.Link, 
                        FormattedUrl = r.FormattedUrl, 
                        Snippet = r.Snippet, 
                        IsDenied = r.IsDenied
                    }).ToList();

            return new SearchPagesResponse { Data = new DataListResponse<SearchResultModel>(items, results.TotalResults) };
        }
    }
}
