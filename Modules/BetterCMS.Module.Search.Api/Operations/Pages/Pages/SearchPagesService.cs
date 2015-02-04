using System.Linq;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Pages.Search;

using BetterCms.Module.Search.Models;
using BetterCms.Module.Search.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Search.Api.Operations.Pages.Pages
{
    public class SearchPagesService : Service, ISearchPagesService
    {
        private readonly ISearchService searchService;

        public SearchPagesService(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public SearchPagesResponse Get(SearchPagesRequest request)
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
