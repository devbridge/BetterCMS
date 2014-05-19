namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    public interface IRedirectsService
    {
        GetRedirectsResponse Get(GetRedirectsRequest request);
        
        PostRedirectResponse Post(PostRedirectRequest request);
    }
}