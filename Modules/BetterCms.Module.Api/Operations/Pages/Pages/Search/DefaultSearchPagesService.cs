using BetterCms.Core.Exceptions;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Search
{
    public class DefaultSearchPagesService : ISearchPagesService
    {
        public SearchPagesResponse Get(SearchPagesRequest request)
        {
            throw new CmsException("Search API interface has no implementation! Install BetterCms.Module.Search.Api for search module API.");
        }
    }
}