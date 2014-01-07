namespace BetterCms.Module.Api.Operations.Pages.Pages.Search
{
    public interface ISearchPagesService
    {
        SearchPagesResponse Get(SearchPagesRequest request);
    }
}